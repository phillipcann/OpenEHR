namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Text.Json.Serialization;
    using System.Text.Json;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// DvDateTimeConverter
    /// </summary>
    /// <remarks>
    /// <para>Handles the Serialization and Deserialization of the DV_DATE_TIME object type when the value is either</para> 
    /// <para>a string value "{ 'time_created': '2024-08-30T21:45:33.389606Z' }"</para>
    /// <para>or a value object "{ 'time_created': { 'value': '2024-08-30T21:45:33.389606Z' }}"  </para>
    /// </remarks>
    public class DvDateTimeConverter : JsonConverter<DvDateTime>
    {
        private readonly ILogger<DvDateTimeConverter> _logger;

        public DvDateTimeConverter(ILogger<DvDateTimeConverter> logger)
        {
            _logger = logger;
        }

        public override DvDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // TODO : This should really be a DvTemporal Converter with DvDateTime as one of the types

            // Handle case where "time_created" is a simple string
            if (reader.TokenType == JsonTokenType.String)
            {
                return new DvDateTime { 
                    Value = reader.GetString() 
                };
            }

            // Handle case where "time_created" is an object
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                // Deserialize the object into a DvDateTime instance
                var doc = JsonDocument.ParseValue(ref reader);
                var value = doc.RootElement.GetProperty("value").GetString();
                
                return new DvDateTime { 
                    Value = value 
                };
            }

            var invalidFormatMessage = $"Invalid format for time_created with TokenType: {reader.TokenType}";
            _logger.LogError(invalidFormatMessage);
            throw new JsonException(invalidFormatMessage);
        }

        public override void Write(Utf8JsonWriter writer, DvDateTime value, JsonSerializerOptions options)
        {
            // Always write as an object for consistency
            writer.WriteStartObject();
            writer.WriteString("value", value.Value);
            writer.WriteEndObject();
        }
    }
}
