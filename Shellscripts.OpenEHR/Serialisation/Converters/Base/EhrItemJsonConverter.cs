namespace Shellscripts.OpenEHR.Serialisation.Converters.Base
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Attribution;

    /// <summary>
    /// Abstract class to provide the capacity to automatically deserialise json 
    /// elements to their appropriate reference model classes
    /// </summary>
    /// <typeparam name="T">Will be used in the JsonConverter as an Object of type T</typeparam>
    public abstract class EhrItemJsonConverter<T> : JsonConverter<T>
        where T : class
    {
        private readonly HashSet<object> _processing = new HashSet<object>(ReferenceEqualityComparer.Instance);

        private readonly ITypeMapLookup _typeMapLookup;
        private readonly ILogger _logger;
        internal ILogger Logger => _logger;

        public EhrItemJsonConverter(ILogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _typeMapLookup = serviceProvider.GetRequiredService<ITypeMapLookup>();
        }

        public override bool CanConvert(Type typeToConvert) => Type.IsAssignableFrom(typeToConvert);

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var isAbstract = typeToConvert.IsAbstract;
            var typeMapAttr = typeToConvert.GetCustomAttribute<TypeMapAttribute>();


            // To prevent infinite loops, remove "typeToConvert" converter from the options.
            var optionsWithoutThis = new JsonSerializerOptions(options);
            optionsWithoutThis.Converters.Remove(this);

            T? returnValue;

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    root.TryGetProperty("_type", out JsonElement typeElement);
                    string idType = typeElement.ValueKind != JsonValueKind.Undefined
                        ? typeElement.GetString() ?? string.Empty
                        : typeMapAttr?.Name ?? string.Empty;

                    Type? targetType = _typeMapLookup.GetTypeByName(idType);

                    if (isAbstract)
                    {
                        targetType ??= typeMapAttr?.DefaultIfAbstract;
                        _logger.LogWarning($"Attempting to Deserialise an Abstract Type: '{typeToConvert.Name}'. Using Default Type: '{targetType?.Name}'");
                    }

                    if (targetType is null)
                    {
                        var unknownTypeMessage = $"Unknown _type: '{idType}'";
                        _logger.LogWarning($"Read() :: {unknownTypeMessage}");

                        throw new JsonException(unknownTypeMessage);
                    }

                    try
                    {
                        var rawText = root.GetRawText();
                        returnValue = (T?)JsonSerializer.Deserialize(rawText, targetType, optionsWithoutThis);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, ex.Message);
                        throw;
                    }
                }
                else
                {
                    var message = "Unable to ascertain root element type or value kind";
                    _logger.LogWarning($"Read() :: {message}");

                    throw new JsonException(message);
                }
            }

            return returnValue;
        }


        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            // Handle Recursive Serialisation Problem.
            if (_processing.Contains(value))
            {
                // TODO : if we are here, we currently wont put any json in the writer so this might cause a problem.
                _logger.LogWarning($"Danger of Serialisation Recursion. Type: {value.GetType().Name}");
                return;
            }

            try
            {
                _processing.Add(value);

                var type = value.GetType();
                var typeMap = value.GetType().GetCustomAttribute<TypeMapAttribute>();

                writer.WriteStartObject();
                writer.WriteString("_type", typeMap?.Name ?? "_UNKNOWN");

                foreach (var property in type.GetProperties())
                {
                    var propValue = property.GetValue(value);
                    var propName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;

                    if (propValue != null)
                    {
                        writer.WritePropertyName(propName);
                        JsonSerializer.Serialize(writer, propValue, property.PropertyType, options);
                    }
                }

                writer.WriteEndObject();
            }
            finally
            {
                _processing.Remove(value);
            }
        }


        //public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        //{
        //    // Handle Recursive Serialisation Problem.
        //    if (_processing.Contains(value))
        //    {
        //        _logger.LogWarning($"Danger of Serialisation Recursion. Type: {value.GetType().Name}");
        //        return;
        //    }

        //    try
        //    {
        //        _processing.Add(value);
        //        var genericTypes = value.GetType().GetGenericArguments();
        //        var converterGenericTypes = string.Join(", ", GetType().GetGenericArguments().Select(t => t.Name));

        //        // What shall we put in the "_type" json property?
        //        var openEhrTypeAttr = value.GetType().GetCustomAttribute<TypeMapAttribute>();
        //        var openEhrType = openEhrTypeAttr?.Name ?? string.Empty;

        //        //// This is required to prevent a circular reference serialising objects
        //        //// that have objects of the same type on it.
        //        //var optionsWithoutConvertor = new JsonSerializerOptions(options);
        //        //optionsWithoutConvertor.Converters.Remove(this);

        //        var tempJson = JsonSerializer.Serialize(value, value.GetType(), options);
        //        using var tempJsonDoc = JsonDocument.Parse(tempJson);
        //        using var enumeratedElements = tempJsonDoc.RootElement.EnumerateObject();

        //        // This is a bit of a hack. As there is a "chance" we are processing a nested converted element
        //        // and we are simply relying on the TypeMap configuration to tell us if we should add the _type
        //        // value. Not ideal.
        //        var containsTypePropAlready = enumeratedElements.Any(p => p.Name.Equals("_type"));
        //        var existingTypes = string.Join(',', enumeratedElements.Where(p => p.Name.Equals("_type")).Select(p => p.Value));

        //        // Add the '_type' property to our Json
        //        writer.WriteStartObject();

        //        if (!containsTypePropAlready)
        //        {
        //            if (openEhrTypeAttr?.OmitFromJson ?? false)
        //            {
        //                _logger.LogInformation($"Serialised Document for '{value.GetType().Name}' deliberatly omitting _type: {openEhrType}");
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrWhiteSpace(openEhrType))
        //                {
        //                    writer.WriteString("_type", openEhrType);
        //                }
        //                else
        //                {
        //                    _logger.LogWarning($"Serialised Document for '{value.GetType().Name}' doesn't have a corresponding OpenEhrType Lookup Value");
        //                }
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogWarning($"Serialised Document already contains _type: OpenEhrType: {openEhrType}. FoundType(s): {existingTypes}");
        //        }

        //        // This adds the properties without nesting the json
        //        foreach (var tempProperty in enumeratedElements)
        //        {
        //            tempProperty.WriteTo(writer);
        //        }

        //        writer.WriteEndObject();
        //    }
        //    finally
        //    {
        //        _processing.Remove(value);
        //    }
        //}
    }
}