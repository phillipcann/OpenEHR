namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataStructures;

    // TODO : Implementation for Element[]
    // TODO : Implementation for Cluster[]

    public class ItemArrayConverter : JsonConverter<Item[]>
    {
        private readonly ILogger<ItemArrayConverter> _logger;
        public ItemArrayConverter(ILogger<ItemArrayConverter> logger)
        {
            _logger = logger;
        }

        public override Item[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            IList<Item> items = new List<Item>();

            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                JsonElement root = document.RootElement;
                var rootArray = root.EnumerateArray();
                
                foreach (var _item in rootArray)
                {
                    if (_item.TryGetProperty("_type", out JsonElement typeElement))
                    {
                        IDictionary<string, Type> typeMap = new Dictionary<string, Type>
                        {
                            { "ELEMENT", typeof(Element) },
                            { "CLUSTER", typeof(Cluster) },
                            { "ITEM", typeof(Item)}
                        };

                        string idType = typeElement.GetString() ?? string.Empty;

                        if (!typeMap.TryGetValue(idType, out Type? targetType))
                        {
                            var unknownTypeMessage = $"Unknown _type: '{idType}'";
                            _logger.LogWarning(unknownTypeMessage);
                            throw new JsonException(unknownTypeMessage);
                        }

                        items.Add((Item)JsonSerializer.Deserialize(_item.GetRawText() ?? string.Empty, targetType, options));
                    }
                    else
                    {
                        throw new JsonException("Unknown _type");
                    }
                }
            }

            return items?.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, Item[] value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
