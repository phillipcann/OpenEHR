namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;


    public class ItemConverter : EhrItemJsonConverter<Item>
    {
        public ItemConverter(ILogger<ItemConverter> logger, IServiceProvider serviceProvider) 
            : base(logger, serviceProvider) { }

        public override Item? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {


            return base.Read(ref reader, typeToConvert, options);
        }

        public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
        {
            base.Write(writer, value, options);
        }
    }
}
