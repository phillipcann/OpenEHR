namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class VersionedObjectConverter : EhrItemJsonConverter<VersionedObject>
    {
        public VersionedObjectConverter(ILogger<VersionedObjectConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }
    }

}
