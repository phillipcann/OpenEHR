namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataStructures;

    // TODO : Implementation for Element[]
    // TODO : Implementation for Cluster[]

    public class ItemArrayConverter : EhrItemJsonArrayConverter<Item>
    {
        public ItemArrayConverter(ILogger<ItemArrayConverter> logger) 
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {
            { "ITEM", typeof(Item) },
            { "ELEMENT", typeof(Element) },
            { "CLUSTER", typeof(Cluster) },            
        };
    }
}