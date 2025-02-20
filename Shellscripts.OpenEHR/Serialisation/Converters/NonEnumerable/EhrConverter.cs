namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;    
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class EhrConverter : EhrItemJsonConverter<Ehr>
    {
        public EhrConverter(ILogger<EhrConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }
    }

}
