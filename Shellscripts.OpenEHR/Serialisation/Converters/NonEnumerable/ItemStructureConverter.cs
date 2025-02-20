namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Attribution;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class ItemStructureConverter : EhrItemJsonConverter<ItemStructure>
    {
        public ItemStructureConverter(ILogger<ItemStructureConverter> logger, IServiceProvider serviceProvider) 
            : base(logger, serviceProvider) { }


        public override void Write(Utf8JsonWriter writer, ItemStructure value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            var typeMap = value.GetType().GetCustomAttribute<TypeMapAttribute>();
            
            writer.WriteStartObject();
            writer.WriteString("_type", typeMap?.Name ?? "_UNKNOWN");

            if (value is ItemSingle singleItem)
            {
                writer.WritePropertyName("item");
                JsonSerializer.Serialize(writer, singleItem.Item, options);
            }
            else if (value is ItemList itemList)
            {
                writer.WritePropertyName("items");
                JsonSerializer.Serialize(writer, itemList.Items, options);
            }
            else if (value is ItemTable itemTable)
            {
                writer.WritePropertyName("rows");
                JsonSerializer.Serialize(writer, itemTable.Rows, options);
            }
            else if (value is ItemTree itemTree)
            {
                writer.WritePropertyName("items");
                JsonSerializer.Serialize(writer, itemTree.Items, options);
            }
            else
            {
                Logger.LogWarning($"Unknown Object: {value.GetType().Name}");
            }

            foreach (var property in type.GetProperties())
            {
                string[] alreadyHandledProps = ["Item", "Items", "Rows"];

                // Already handled
                if (alreadyHandledProps.Contains(property.Name)) 
                    continue;

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
    }
}