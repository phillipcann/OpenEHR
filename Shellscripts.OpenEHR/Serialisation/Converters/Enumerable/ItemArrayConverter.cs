namespace Shellscripts.OpenEHR.Serialisation.Converters.Enumerable
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class ItemArrayConverter : EhrItemJsonArrayConverter<Item>        
    {
        public ItemArrayConverter(ILogger<ItemArrayConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }
    }
}