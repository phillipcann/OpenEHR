namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class PartyProxyConverter : EhrItemJsonConverter<PartyProxy>
    {
        public PartyProxyConverter(ILogger<PartyProxyConverter> logger) 
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {
            { "PARTY_PROXY", typeof(PartyProxy) },
            { "PARTY_SELF", typeof(PartySelf) },
            { "PARTY_IDENTIFIED", typeof(PartyIdentified) },
            { "PARTY_RELATED", typeof(PartyRelated) }
        };        
    }
}