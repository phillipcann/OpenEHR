namespace Shellscripts.OpenEHR.Tests.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;    
    using System.Text.Json;    

    public class JsonEqualityComparer
    {
        public static bool AreJsonStringsEqual(string json1, string json2)
        {
            using var doc1 = JsonDocument.Parse(json1);
            using var doc2 = JsonDocument.Parse(json2);

            return _jsonElementEqualityComparer(doc1.RootElement, doc2.RootElement);
        }

        private static bool _jsonElementEqualityComparer(JsonElement element1, JsonElement element2)
        {
            if (element1.ValueKind != element2.ValueKind)
                return false;

            switch (element1.ValueKind)
            {
                case JsonValueKind.Object:
                    var properties1 = element1.EnumerateObject().OrderBy(p => p.Name);
                    var properties2 = element2.EnumerateObject().OrderBy(p => p.Name);

                    return properties1.SequenceEqual(properties2, new JsonPropertyEqualityComparer());

                case JsonValueKind.Array:
                    var items1 = element1.EnumerateArray();
                    var items2 = element2.EnumerateArray();

                    return items1.SequenceEqual(items2, new JsonElementEqualityComparer());

                default:
                    return element1.ToString() == element2.ToString();
            }
        }

        class JsonPropertyEqualityComparer : IEqualityComparer<JsonProperty>
        {
            public bool Equals(JsonProperty x, JsonProperty y)
            {
                return x.Name == y.Name && _jsonElementEqualityComparer(x.Value, y.Value);                
            }

            public int GetHashCode(JsonProperty obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        class JsonElementEqualityComparer : IEqualityComparer<JsonElement>
        {
            public bool Equals(JsonElement x, JsonElement y)
            {
                return _jsonElementEqualityComparer(x, y);
            }

            public int GetHashCode(JsonElement obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }


}
