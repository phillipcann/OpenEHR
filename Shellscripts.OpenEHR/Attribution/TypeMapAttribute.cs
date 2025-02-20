namespace Shellscripts.OpenEHR.Attribution
{
    /// <summary>
    /// This attribute will provide the capacity to specify what the json _type value will
    /// be reflected as when a class is serialised    
    /// </summary>

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TypeMapAttribute : Attribute
    {
        public string Name { get; set; }
        public bool OmitFromJson { get; set; }
        public Type? DefaultIfAbstract { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultIfAbstract"></param>
        /// <param name="omitFromJson"></param>
        public TypeMapAttribute(string name, Type? defaultIfAbstract = null, bool omitFromJson = false)
        {
            Name = name;
            OmitFromJson = omitFromJson;
            DefaultIfAbstract = defaultIfAbstract;
        }
    }
}
