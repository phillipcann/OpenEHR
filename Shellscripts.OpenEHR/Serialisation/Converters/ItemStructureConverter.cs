namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataStructures;

    public class ItemStructureConverter : EhrItemJsonConverter<ItemStructure>
    {
        public ItemStructureConverter(ILogger<ItemStructureConverter> logger) 
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {
            { "ITEM_STRUCTURE", typeof(ItemStructure) },
            { "ITEM_LIST", typeof(ItemList)},
            { "ITEM_TABLE", typeof(ItemTable)},
            { "ITEM_TREE", typeof(ItemTree)}
        };
    }
}
