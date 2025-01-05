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
    }
}
