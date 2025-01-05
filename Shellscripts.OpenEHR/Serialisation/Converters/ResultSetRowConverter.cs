namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Models.PlatformServiceModel;


    public class ResultSetRowConverter : JsonConverter<ResultSetRow[]>
    {
        public override ResultSetRow[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected StartArray token.");

            var rows = new List<ResultSetRow>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var values = JsonSerializer.Deserialize<object[]>(ref reader, options);
                    rows.Add(new ResultSetRow { Values = values });
                }
                else
                {
                    throw new JsonException("Expected StartArray token for a row.");
                }
            }

            return rows.ToArray();
        }

        public override void Write(Utf8JsonWriter writer, ResultSetRow[] value, JsonSerializerOptions options)
        {
            // This will likely NEVER need a reason to turn into Json as we will only ever need to be 
            // deserialising the result FROM a Ehr Query Api call.
            throw new NotImplementedException();
        }
    }
}
