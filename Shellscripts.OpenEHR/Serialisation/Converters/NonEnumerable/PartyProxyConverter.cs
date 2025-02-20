namespace Shellscripts.OpenEHR.Serialisation.Converters.NonEnumerable
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class PartyProxyConverter : EhrItemJsonConverter<PartyProxy>
    {
        public PartyProxyConverter(ILogger<PartyProxyConverter> logger, IServiceProvider serviceProvider)
            : base(logger, serviceProvider) { }

    }
}