namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Text.Json.Serialization;
    using System.Text.Json;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Microsoft.Extensions.Logging;

    public class ObjectRefConverter : JsonConverter<ObjectRef>
    {
        private readonly ILogger<ObjectRefConverter> _logger;

        public ObjectRefConverter(ILogger<ObjectRefConverter> logger)
        {
            _logger = logger;
        }

        public override ObjectRef Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the JSON into a JsonDocument
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                // Create a new ObjectRef instance
                root.TryGetProperty("namespace", out JsonElement @namespace);
                root.TryGetProperty("type", out JsonElement @type);

                var objRef = new ObjectRef() { Namespace = null, Type = null };
                if (@namespace.ValueKind != JsonValueKind.Undefined) objRef.Namespace = @namespace.GetString();
                if (@type.ValueKind != JsonValueKind.Undefined) objRef.Type = @type.GetString();

                // Check the _type field in the "id" property to determine the type.
                JsonElement idElement = new JsonElement();
                if (root.TryGetProperty("id", out idElement))
                {
                    // Id is value for HIER_OBJECT_ID
                }
                else if (root.TryGetProperty("uid", out idElement))
                {
                    // Uid is value for OBJECT_VERSION_ID
                }
                else
                {
                    var unknownIdElementMessage = "Unable to extract the Id section where the _type field lives";
                    _logger.LogError(unknownIdElementMessage);
                    throw new JsonException(unknownIdElementMessage);
                }

                if (idElement.TryGetProperty("_type", out JsonElement typeElement))
                {
                    string idType = typeElement.GetString() ?? string.Empty;

                    IDictionary<string, Type> typeMap = new Dictionary<string, Type>
                    {
                        { "HIER_OBJECT_ID", typeof(HierObjectId) },
                        { "UID_BASED_ID", typeof(UidBasedId)},
                        { "OBJECT_VERSION_ID", typeof(ObjectVersionId)},
                        { "ARCHETYPE_ID", typeof(ArchetypeId)},
                        { "TEMPLATE_ID", typeof(TemplateId)},
                        { "TERMINOLOGY_ID", typeof(TerminologyId)},
                        { "GENERIC_ID", typeof(GenericId) }                        
                    };

                    if (!typeMap.ContainsKey(idType))
                    {
                        var unknownTypeMessage = $"Unknown _type: '{idType}'";
                        _logger.LogWarning(unknownTypeMessage);
                        throw new JsonException(unknownTypeMessage);
                    }

                    objRef.Id = (ObjectId)JsonSerializer.Deserialize(idElement.GetRawText() ?? string.Empty, typeMap[idType]);
                }
                else
                {
                    var missingTypeMessage = "Missing _type field in id.";
                    _logger.LogError(missingTypeMessage);
                    throw new JsonException(missingTypeMessage);
                }

                return objRef;
            }
        }

        public override void Write(Utf8JsonWriter writer, ObjectRef value, JsonSerializerOptions options)
        {
            // Serialize ObjectRef into JSON
            writer.WriteStartObject();
            writer.WriteString("namespace", value.Namespace);
            writer.WriteString("type", value.Type);

            // Serialize the Id property, including the _type field
            writer.WritePropertyName("id");
            JsonSerializer.Serialize(writer, value.Id, value.Id.GetType(), options);
            writer.WriteEndObject();
        }
    }
}
