namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataStructures;


    public class ItemStructureConverter : JsonConverter<ItemStructure>
    {
        private readonly ILogger<ItemStructureConverter> _logger;

        public ItemStructureConverter(ILogger<ItemStructureConverter> logger)
        {
            _logger = logger;
        }

        public override ItemStructure? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ItemStructure itemStructure;

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("_type", out JsonElement typeElement))
                {
                    IDictionary<string, Type> typeMap = new Dictionary<string, Type>
                    {
                        { "ITEM_STRUCTURE", typeof(ItemStructure) },
                        { "ITEM_LIST", typeof(ItemList)},
                        { "ITEM_TABLE", typeof(ItemTable)},
                        { "ITEM_TREE", typeof(ItemTree)}
                    };

                    string idType = typeElement.GetString() ?? string.Empty;

                    if (!typeMap.TryGetValue(idType, out Type? targetType))
                    {
                        var unknownTypeMessage = $"Unknown _type: '{idType}'";
                        _logger.LogWarning(unknownTypeMessage);
                        throw new JsonException(unknownTypeMessage);
                    }

                    itemStructure = (ItemStructure)JsonSerializer.Deserialize(root.GetRawText() ?? string.Empty, targetType, options);
                }
                else
                {
                    throw new JsonException("Unknown _type");
                }
            }

            return itemStructure;
        }



        public override void Write(Utf8JsonWriter writer, ItemStructure value, JsonSerializerOptions options)
        {
            // TODO : Create Implementation
            throw new NotImplementedException();
        }
    }
}
