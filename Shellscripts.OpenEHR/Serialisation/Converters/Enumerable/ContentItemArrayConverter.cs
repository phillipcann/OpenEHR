namespace Shellscripts.OpenEHR.Serialisation.Converters.Enumerable
{
    using System;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;    
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class ContentItemArrayConverter : EhrItemJsonArrayConverter<ContentItem>        
    {
        public ContentItemArrayConverter(ILogger<ContentItemArrayConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }

    }
}