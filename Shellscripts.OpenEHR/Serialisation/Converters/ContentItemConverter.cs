namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class ContentItemConverter : EhrItemJsonConverter<ContentItem>
    {
        public ContentItemConverter(ILogger<ContentItemConverter> logger) 
            : base(logger) {  }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>()
        {
            { "CONTENT_ITEM", typeof(ContentItem) },            
            { "SECTION", typeof(Section) },
            { "ENTRY", typeof(Entry) },
            { "OBSERVATION", typeof(Observation) }
        };
    }    
}