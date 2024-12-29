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

                Type currentType = currentObject.GetType();
                PropertyInfo? propertyInfo = currentType.GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    throw new ArgumentException($"Property '{property}' not found on type '{currentType.FullName}'.");
                }

                currentObject = propertyInfo.GetValue(currentObject);
            }

            return currentObject;
        }

    }
}
