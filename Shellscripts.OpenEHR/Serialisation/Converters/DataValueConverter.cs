namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataTypes;


    public class DataValueConverter : JsonConverter<DataValue>
    {
        private readonly ILogger<DataValueConverter> _logger;

        public DataValueConverter(ILogger<DataValueConverter> logger)
        {
            _logger = logger;
        }

        public override DataValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DataValue value;

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("_type", out JsonElement typeElement))
                {
                    // TODO : This list might not be exhaustive
                    IDictionary<string, Type> typeMap = new Dictionary<string, Type>
                    {
                        { "DATA_VALUE", typeof(DataValue) },
                        { "DV_BOOLEAN", typeof(DvBoolean) },
                        { "DV_STATE", typeof(DvState) },
                        { "DV_IDENTIFIER", typeof(DvIdentifier) },
                        { "DV_TEXT", typeof(DvText) },
                        { "DV_PARAGRAPH", typeof(DvParagraph) },
                        { "DV_ORDERED", typeof(DvOrdered) },
                        { "DV_INTERVAL", typeof(DvInterval) },
                        { "DV_TIME_SPECIFICATION", typeof(DvTimeSpecification) },
                        { "DV_ENCAPSULATED", typeof(DvEncapsulated) },
                        { "DV_URI", typeof(DvUri) },
                        { "DV_CODED_TEXT", typeof(DvCodedText) }
                    };

                    string idType = typeElement.GetString() ?? string.Empty;

                    if (!typeMap.TryGetValue(idType, out Type? targetType))
                    {
                        var unknownTypeMessage = $"Unknown _type: '{idType}'";
                        _logger.LogWarning(unknownTypeMessage);
                        throw new JsonException(unknownTypeMessage);
                    }

                    value = (DataValue)JsonSerializer.Deserialize(root.GetRawText() ?? string.Empty, targetType, options);
                }
                else
                {
                    throw new JsonException("Unknown _type");
                }

                // DataValue                
                // DvBoolean
                // DvState
                // DvIdentifier
                // DvText
                // DvParagraph
                // DvOrdered
                // DvInterval
                // DvTimeSpecification
                // DvEncapsulated
                // DvUri
            }

            return value;
        }

        public override void Write(Utf8JsonWriter writer, DataValue value, JsonSerializerOptions options)
        {
            // TODO : Create Implementation
            throw new NotImplementedException();
        }
    }
}
