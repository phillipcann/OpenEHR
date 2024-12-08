namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Text.Json.Serialization;
    using System.Text.Json;
    using Shellscripts.OpenEHR.Models.DataTypes;

    public class DvDateTimeConverter : JsonConverter<DvDateTime>
    {
        public override DvDateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new DvDateTime
                {
                    Value = reader.GetString()
                };
            }

            throw new JsonException("Expected a string for DvDateTime.");
        }

        public override void Write(Utf8JsonWriter writer, DvDateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}
