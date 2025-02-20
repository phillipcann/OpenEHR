namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;
    using System.Text.Json;

    public class ObjectRefConverter : EhrItemJsonConverter<ObjectRef>        
    {
        public ObjectRefConverter(ILogger<ObjectRefConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }
    }
}