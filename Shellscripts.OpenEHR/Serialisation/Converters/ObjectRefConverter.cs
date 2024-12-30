namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;

    public class ObjectRefConverter : EhrItemJsonConverter<ObjectRef>
    {
        public ObjectRefConverter(ILogger<ObjectRefConverter> logger) 
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {
            { "OBJECT_REF", typeof(ObjectRef) },
            { "PARTY_REF", typeof(PartyRef)},
            { "LOCATABLE_REF", typeof(LocatableRef) }
        };
    }
}