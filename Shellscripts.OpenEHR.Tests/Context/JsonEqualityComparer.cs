namespace Shellscripts.OpenEHR.Tests.Context
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;    
    using System.Text.Json;
    using Microsoft.Extensions.Logging;

    public class JsonEqualityComparer
    {

        public static bool AreJsonStringsEqual(string json1, string json2)
        {
            using var doc1 = JsonDocument.Parse(json1);
            using var doc2 = JsonDocument.Parse(json2);

            return jsonElementEqualityComparer(doc1.RootElement, doc2.RootElement);
        }

        private static bool jsonElementEqualityComparer(JsonElement element1, JsonElement element2)
        {
            if (element1.ValueKind != element2.ValueKind)
            {
                Debug.WriteLine("JsonEqualityComparer :: Element 1 and Element 2 are mismatched ValueKinds");
                return false;
            }

            switch (element1.ValueKind)
            {
                case JsonValueKind.Object:
                    var properties1 = element1.EnumerateObject().OrderBy(p => p.Name);
                    var properties2 = element2.EnumerateObject().OrderBy(p => p.Name);

                    var propertySequenceEqual = properties1.SequenceEqual(properties2, new JsonPropertyEqualityComparer());

                    if (!propertySequenceEqual)
                    {
                        Debug.WriteLine("JsonEqualityComparer :: Element 1 and Element 2 have mismatched property sequences");
                    }

                    return propertySequenceEqual;

                case JsonValueKind.Array:
                    var items1 = element1.EnumerateArray();
                    var items2 = element2.EnumerateArray();

                    var itemArraySequenceEqual = items1.SequenceEqual(items2, new JsonElementEqualityComparer());

                    if (!itemArraySequenceEqual)
                    {
                        Debug.WriteLine("JsonEqualityComparer :: Element 1 and Element 2 have mismatched array sequences");
                    }

                    return itemArraySequenceEqual;

                default:
                    var elementMatch = element1.ToString() == element2.ToString();

                    if (!elementMatch)
                    {
                        Debug.WriteLine("JsonEqualityComparer :: Element 1 and Element 2 values do not match");
                    }

                    return elementMatch;
            }
        }

        class JsonPropertyEqualityComparer : IEqualityComparer<JsonProperty>
        {
            public bool Equals(JsonProperty x, JsonProperty y)
            {
                return x.Name == y.Name && jsonElementEqualityComparer(x.Value, y.Value);                
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
                return jsonElementEqualityComparer(x, y);
            }

            public int GetHashCode(JsonElement obj)
            {
                return obj.ToString().GetHashCode();
            }
        }
    }


}
