namespace Shellscripts.OpenEHR.Serialisation.Converters.Base
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Abstract class to provide the capacity to automatically deserialise json 
    /// elements to their appropriate reference model classes
    /// </summary>
    /// <typeparam name="T">Will be used in the JsonConverter as an Object of type T</typeparam>
    public abstract class EhrItemJsonConverter<T> : JsonConverter<T>
        where T : class, new()
    {
        private readonly ILogger _logger;
        internal ILogger Logger => _logger;
        public abstract IDictionary<string, Type> TypeMap { get; }

        public EhrItemJsonConverter(ILogger logger)
        {
            _logger = logger;
        }

        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (TypeMap == null || !TypeMap.Any())
                throw new InvalidOperationException("TypeMap MUST have an implementation");

            // To prevent infinite loops, remove "typeToConvert" converter from the options.
            var optionsWithoutThis = new JsonSerializerOptions(options);
            optionsWithoutThis.Converters.Remove(this);

            T returnValue = new();

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                if (root.TryGetProperty("_type", out JsonElement typeElement))
                {
                    string idType = typeElement.GetString() ?? string.Empty;

                    if (!TypeMap.TryGetValue(idType, out Type? targetType))
                    {
                        var unknownTypeMessage = $"Unknown _type: '{idType}'";
                        _logger.LogWarning($"Read() :: {unknownTypeMessage}");

                        throw new JsonException(unknownTypeMessage);
                    }

                    returnValue = (T)JsonSerializer.Deserialize(root.GetRawText(), targetType, optionsWithoutThis);
                }
                else
                {
                    var message = "Unable to ascertain root element type";
                    _logger.LogWarning($"Read() :: {message}");

                    throw new JsonException(message);
                }
            }

            return returnValue;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            if (TypeMap == null || !TypeMap.Any())
                throw new InvalidOperationException("TypeMap MUST have an implementation");

            var optionsWithoutConvertor = new JsonSerializerOptions(options);
            optionsWithoutConvertor.Converters.Remove(this);

            var contentType = value.GetType();
            JsonSerializer.Serialize(writer, value, contentType, optionsWithoutConvertor);
        }
    }
}