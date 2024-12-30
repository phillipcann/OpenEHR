namespace Shellscripts.OpenEHR.Tests
{
    using System;
    using System.Reflection;

    public static class HelperExtensions
    {
        public static object? GetNestedPropertyValue(this object obj, string propertyPath)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (string.IsNullOrWhiteSpace(propertyPath))
                throw new ArgumentException("Property path cannot be null or empty.", nameof(propertyPath));

            string[] properties = propertyPath.Split('.');
            object? currentObject = obj;

            foreach (string property in properties)
            {
                if (currentObject == null)
                {
                    return null; // The intermediate property is null, so the final value is also null.
                }

                // Check if the property contains array indexing, e.g., "Items[0]"
                int arrayStartIndex = property.IndexOf('[');
                if (arrayStartIndex >= 0)
                {
                    // Extract property name and index
                    string propertyName = property.Substring(0, arrayStartIndex);
                    int arrayEndIndex = property.IndexOf(']');
                    if (arrayEndIndex < 0 || arrayEndIndex <= arrayStartIndex + 1)
                        throw new ArgumentException($"Invalid array index syntax in '{property}'.");

                    string indexString = property.Substring(arrayStartIndex + 1, arrayEndIndex - arrayStartIndex - 1);
                    if (!int.TryParse(indexString, out int arrayIndex))
                        throw new ArgumentException($"Invalid array index '{indexString}' in '{property}'.");

                    // Get the property value
                    Type currentType = currentObject.GetType();
                    PropertyInfo? propertyInfo = currentType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                        throw new ArgumentException($"Property '{propertyName}' not found on type '{currentType.FullName}'.");

                    object? propertyValue = propertyInfo.GetValue(currentObject);

                    // Ensure the property value is a collection and get the indexed element
                    if (propertyValue is System.Collections.IList list)
                    {
                        if (arrayIndex < 0 || arrayIndex >= list.Count)
                            throw new IndexOutOfRangeException($"Index {arrayIndex} is out of range for property '{propertyName}'.");

                        currentObject = list[arrayIndex];
                    }
                    else
                    {
                        throw new ArgumentException($"Property '{propertyName}' is not a collection.");
                    }
                }
                else
                {
                    // Regular property access
                    Type currentType = currentObject.GetType();
                    PropertyInfo? propertyInfo = currentType.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                        throw new ArgumentException($"Property '{property}' not found on type '{currentType.FullName}'.");

                    currentObject = propertyInfo.GetValue(currentObject);
                }
            }

            return currentObject;
        }


    }
}
