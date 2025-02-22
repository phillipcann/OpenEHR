namespace Shellscripts.OpenEHR.Serialisation.Converters.Base
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Attribution;

    /// <summary>
    /// Abstract class to provide the capacity to automatically deserialise json array 
    /// elements to their appropriate reference model classes
    /// </summary>
    /// <typeparam name="T">Will be used in the JsonConverter as an Enumerable of type T</typeparam>
    public abstract class EhrItemJsonArrayConverter<T> : JsonConverter<T[]>
        where T : class
    {
        private readonly ITypeMapLookup _typeMapLookup;
        private readonly ILogger _logger;
        internal ILogger Logger => _logger;

        public EhrItemJsonArrayConverter(ILogger logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _typeMapLookup = serviceProvider.GetRequiredService<ITypeMapLookup>();
        }

        public override bool CanConvert(Type typeToConvert) => Type.IsAssignableFrom(typeToConvert);

        public override T[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            IList<T> returnList = new List<T>();

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;
                var rootArray = root.EnumerateArray();

                foreach (var _item in rootArray)
                {
                    if (_item.TryGetProperty("_type", out JsonElement typeElement))
                    {
                        string idType = typeElement.ValueKind != JsonValueKind.Undefined
                            ? typeElement.GetString() ?? string.Empty
                            : typeToConvert.GetCustomAttribute<TypeMapAttribute>()?.Name ?? string.Empty;

                        Type? targetType = _typeMapLookup.GetTypeByName(idType);

                        if (targetType is null)
                        {
                            var unknownTypeMessage = $"Unknown _type: '{idType}'";
                            _logger.LogWarning($"Read() :: {unknownTypeMessage}");

                            throw new JsonException(unknownTypeMessage);
                        }

                        returnList.Add((T)JsonSerializer.Deserialize(_item.GetRawText(), targetType, options));
                    }
                    else
                    {
                        var message = "Unable to ascertain root element type";
                        _logger.LogWarning($"Read() :: {message}");

                        throw new JsonException(message);
                    }
                }
            }

            return returnList.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var item in value)
            {
                var itemType = item.GetType();
                var typeMap = item.GetType().GetCustomAttribute<TypeMapAttribute>();

                //_logger.LogInformation($"\tWriting Json: ItemType: {itemType.Name}. TypeMap: {typeMap?.Name}");

                writer.WriteStartObject();                

                // This serialize method "should" give us the _type parameter for the single item we are serialising...
                var json = JsonSerializer.Serialize(item, itemType, options);

                using (var doc = JsonDocument.Parse(json))
                {
                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        prop.WriteTo(writer);
                    }
                }

                writer.WriteEndObject();
            }
            
            writer.WriteEndArray();
        }
    }
}