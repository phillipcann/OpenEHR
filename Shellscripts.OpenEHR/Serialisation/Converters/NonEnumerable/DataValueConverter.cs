namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class DataValueConverter : EhrItemJsonConverter<DataValue>        
    {
        public DataValueConverter(ILogger<DataValueConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }

        public override DataValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            const string REGEX_ISO_8601 = @"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?Z$";

            // Special Case for DateTime where we are simply looking at a String value and there is no _type to go from

            var readerCopy = reader;
            using (JsonDocument document = JsonDocument.ParseValue(ref readerCopy))
            {
                var root = document.RootElement;

                if (root.ValueKind == JsonValueKind.String)
                {
                    var stringValue = root.GetString();

                    if (Regex.IsMatch(stringValue, REGEX_ISO_8601))
                    {
                        return new DvDateTime() { Value = stringValue };
                    }
                }
            }

            return base.Read(ref reader, typeToConvert, options);
        }
    }
}