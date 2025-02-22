namespace Shellscripts.OpenEHR.Extensions
{
    using System.Reflection;

    public static class ObjectExtensions
    {

        /// <summary>
        /// IsAnonymousType
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(this object obj)
        {
            if (obj == null) 
                return false;

            Type type = obj.GetType();

            // Check if the type has the CompilerGeneratedAttribute
            bool isCompilerGenerated = Attribute.IsDefined(type, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute));

            // Check if the type is sealed, not public, and derives directly from object
            bool meetsCriteria = isCompilerGenerated
                                 && type.IsGenericType
                                 && (type.Attributes & TypeAttributes.Public) == 0
                                 && type.Name.Contains("AnonymousType")
                                 && type.BaseType == typeof(object);

            return meetsCriteria;
        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <remarks>
        /// Copies all properties that match name and of assignable type from one object to another
        /// </remarks>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="throwOnNull"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Clone(this object from, object to, bool throwOnNull = false)
        {
            // Nothing to do
            if (from == null && to == null)
            {
                if (throwOnNull)
                    throw new ArgumentNullException($"from and to cannot be null");

                return;
            }

            // One property is null, the other is not. Unable to continue
            if (from == null || to == null)
            {
                if (throwOnNull)
                    throw new ArgumentNullException($"either from or to is null");

                return;
            }

            Type sourceType = from.GetType();
            Type targetType = to.GetType();

            PropertyInfo[] sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] targetProperties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProp in sourceProperties)
            {
                // TODO : We need to check if the target property is an Abstract Type with a Generic Type Argument (i.e. Event<ItemStructure>) that it will clone correctly (or at all)
                var targetProp = Array.Find(targetProperties, p => p.Name == sourceProp.Name && p.PropertyType.IsAssignableFrom(sourceProp.PropertyType));

                if (targetProp != null && targetProp.CanWrite)
                {
                    object value = sourceProp.GetValue(from);
                    targetProp.SetValue(to, value);
                }
            }
        }
    }
}
