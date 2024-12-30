namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.Ehr;

    public class ContentItemArrayConverter : EhrItemJsonArrayConverter<ContentItem>
    {
        public ContentItemArrayConverter(ILogger<ContentItemArrayConverter> logger) 
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>()
        {
            { "CONTENT_ITEM", typeof(ContentItem) },
            { "SECTION", typeof(Section) },
            { "OBSERVATION", typeof(Observation) },
        };
    }
}
