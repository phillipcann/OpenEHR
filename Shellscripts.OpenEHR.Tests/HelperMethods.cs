namespace Shellscripts.OpenEHR.Tests
{
    using System;
    using System.Text.Json;
    

    public class HelperMethods
    {

        /// <summary>
        /// Check that a value at a given json path location matches an expected value and type which
        /// also includes possible expected null values
        /// </summary>
        /// <param name="actual_json"></param>
        /// <param name="json_path"></param>
        /// <param name="expected_value"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static void AsssertJsonValueEquality(string actual_json, string json_path, object? expected_value)
        {
            if (string.IsNullOrWhiteSpace(actual_json))
                throw new ArgumentException("JSON cannot be null or empty", nameof(actual_json));

            if (string.IsNullOrWhiteSpace(json_path))
                throw new ArgumentException("JSONPath cannot be null or empty", nameof(json_path));

            // Parse JSON
            using JsonDocument doc = JsonDocument.Parse(actual_json);
            var element = doc.RootElement;

            // Locate the value using the path
            var extractedElement = GetJsonElementByPath(element, json_path);

            if (extractedElement is null)
                throw new ArgumentException($"No match found for JSON path: {json_path}");

            // Convert extracted element to object
            object? actualValue = ConvertJsonElementToDotNetType(extractedElement.Value);

            // Type check
            if (actualValue?.GetType() != expected_value?.GetType())
                throw new InvalidOperationException($"Type mismatch: Path: {json_path}. Expected {expected_value?.GetType()}. Found {actualValue?.GetType()}");

            // Value equality check
            if (!actualValue?.Equals(expected_value) ?? expected_value != null)
                throw new InvalidOperationException($"Value mismatch: Path: {json_path}. Expected: {expected_value}. Found: {actualValue}");
        }

        private static JsonElement? GetJsonElementByPath(JsonElement element, string jsonPath)
        {
            string[] segments = jsonPath.Trim('$').Split('.', StringSplitOptions.RemoveEmptyEntries);

            foreach (var segment in segments)
            {
                if (segment.Contains("[") && segment.Contains("]")) // Array index case
                {
                    var arrayParts = segment.Split('[', ']');
                    string propertyName = arrayParts[0]; // Property name before `[`
                    if (!int.TryParse(arrayParts[1], out int index))
                        throw new ArgumentException($"Invalid array index in path: {segment}");

                    if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty(propertyName, out JsonElement arrayElement) &&
                        arrayElement.ValueKind == JsonValueKind.Array && index >= 0 && index < arrayElement.GetArrayLength())
                    {
                        element = arrayElement[index]; // Extract the indexed array element
                    }
                    else
                    {
                        return null; // Path is incorrect
                    }
                }
                else // Regular property case
                {
                    if (element.ValueKind == JsonValueKind.Object && element.TryGetProperty(segment, out JsonElement foundElement))
                    {
                        element = foundElement;
                    }
                    else
                    {
                        return null; // Property not found
                    }
                }
            }

            return element;
        }


        private static object? ConvertJsonElementToDotNetType(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString(),
                JsonValueKind.Number => ConvertJsonNumber(element),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                _ => throw new InvalidOperationException($"Unsupported JSON type: {element.ValueKind}")
            };
        }

        private static object ConvertJsonNumber(JsonElement element)
        {
            // If it's a whole number, check if it fits in an int or long
            if (element.TryGetInt32(out int intValue))
                return intValue;

            if (element.TryGetInt64(out long longValue))
                return longValue;

            // Otherwise, return as double (for floating-point numbers)
            return element.GetDouble();
        }
    }
}
