namespace Shellscripts.OpenEHR.Serialisation.Converters.Base
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Abstract class to provide the capacity to automatically deserialise json array 
    /// elements to their appropriate reference model classes
    /// </summary>
    /// <typeparam name="T">Will be used in the JsonConverter as an Enumerable of type T</typeparam>
    public abstract class EhrItemJsonArrayConverter<T> : JsonConverter<T[]>
        where T : class, new()
    {
        private readonly ILogger _logger;
        internal ILogger Logger => _logger;
        public abstract IDictionary<string, Type> TypeMap { get; }

        public EhrItemJsonArrayConverter(ILogger logger)
        {
            _logger = logger;
        }

        public override T[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (TypeMap == null || !TypeMap.Any())
                throw new InvalidOperationException("TypeMap MUST have an implementation");

            IList<T> returnList = new List<T>();

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;
                var rootArray = root.EnumerateArray();

                foreach (var _item in rootArray)
                {
                    if (_item.TryGetProperty("_type", out JsonElement typeElement))
                    {
                        string idType = typeElement.GetString() ?? string.Empty;

                        if (!TypeMap.TryGetValue(idType, out Type? targetType))
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
            _logger.LogInformation($"Writing Json: Converter: {GetType().Name}. ValueType: {value.GetType().Name}");

            if (TypeMap == null || !TypeMap.Any())
                throw new InvalidOperationException("TypeMap MUST have an implementation");

            //var optionsWithoutConvertor = new JsonSerializerOptions(options);
            //optionsWithoutConvertor.Converters.Remove(this);


            writer.WriteStartArray();
            foreach (var item in value)
            {
                var itemType = item.GetType();
                _logger.LogInformation($"\tWriting Json: ItemType: {itemType.Name}");

                writer.WriteStartObject();

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
