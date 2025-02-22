using Shellscripts.OpenEHR.Extensions;

namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;
    using System.Reflection;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Attribution;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;    

    public class EventConverter<T> : EhrItemJsonConverter<Event<T>>
        where T : ItemStructure
    {
        public EventConverter(ILogger<EventConverter<T>> logger, IServiceProvider serviceProvider) 
            : base(logger, serviceProvider)
        {
        }

        public override Event<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Event - abstract  (data property)
            // Point Event 
            // Interval Event
            Type? eventType;

            TypeMapAttribute? eventTypeAttr = typeof(Event<>).GetCustomAttribute<TypeMapAttribute>();

            var optionsWithoutThis = new JsonSerializerOptions(options);
            optionsWithoutThis.Converters.Remove(this);

            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            
            var root = doc.RootElement;

            // What Type of Event? (PointEvent / IntervalEvent)
            bool eventTypeFound = root.TryGetProperty("_type", out JsonElement typeElement);
            eventType = eventTypeFound
                ? TypeMapLookup.GetTypeByName(typeElement.GetString() ?? string.Empty)
                : eventTypeAttr?.DefaultIfAbstract;

            if (eventType is null)
                throw new JsonException("Unable to infer EventType from supplied Json");

            typeToConvert = eventType.MakeGenericType(typeof(ItemStructure));

            var deserialisedObject = JsonSerializer.Deserialize(doc, typeToConvert, optionsWithoutThis);
            var convertedObject = deserialisedObject as Event<ItemStructure>;

            return convertedObject as Event<T>;
        }
    }
}
