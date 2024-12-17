namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.CommonInformation;

    public class PartyProxyConverter : JsonConverter<PartyProxy>
    {

        private readonly ILogger<PartyProxyConverter> _logger;

        public PartyProxyConverter(ILogger<PartyProxyConverter> logger)
        {
            _logger = logger;
        }

        public override PartyProxy? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            PartyProxy partyProxy;

            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = jsonDocument.RootElement;

                if (root.TryGetProperty("_type", out JsonElement typeElement))
                {
                    IDictionary<string, Type> typeMap = new Dictionary<string, Type>
                    {
                        { "PARTY_PROXY", typeof(PartyProxy) },
                        { "PARTY_SELF", typeof(PartySelf) },
                        { "PARTY_IDENTIFIED", typeof(PartyIdentified) },
                        { "PARTY_RELATED", typeof(PartyRelated) }
                    };

                    string idType = typeElement.GetString() ?? string.Empty;
                    
                    if (!typeMap.TryGetValue(idType, out Type? targetType))
                    {
                        var unknownTypeMessage = $"Unknown _type: '{idType}'";
                        _logger.LogWarning(unknownTypeMessage);
                        throw new JsonException(unknownTypeMessage);
                    }

                    partyProxy = (PartyProxy)JsonSerializer.Deserialize(root.GetRawText() ?? string.Empty, targetType);
                }
                else
                {
                    // unable to ascertain the type....
                    throw new JsonException("Unknown _type");
                }
            }

            return partyProxy;
        }

        public override void Write(Utf8JsonWriter writer, PartyProxy value, JsonSerializerOptions options)
        {
            // TODO : Check the Writing piece of this puzzle works.

            // Serialize ObjectRef into JSON
            writer.WriteStartObject();
            //writer.WritePropertyName("party_ref");
            //JsonSerializer.Serialize(writer, value.ExternalReference, value.ExternalReference.GetType(), options);
            writer.WriteEndObject();
        }
    }
}
