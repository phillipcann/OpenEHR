namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.Ehr;

    public class ContentItemConverter : EhrItemJsonConverter<ContentItem>
    {
        public ContentItemConverter(ILogger<ContentItemConverter> logger) : base(logger) {  }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>()
        {
            { "CONTENT_ITEM", typeof(ContentItem) },
            { "OBSERVATION", typeof(Observation) },
            { "SECTION", typeof(Section) },
        };
    }
}
