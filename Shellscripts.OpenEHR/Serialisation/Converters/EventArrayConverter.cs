namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class EventArrayConverter : EhrItemJsonArrayConverter<Event<ItemStructure>>
    {
        public EventArrayConverter(ILogger<EventArrayConverter> logger) 
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {            
            { "EVENT", typeof(Event<ItemStructure>) },
            { "INTERVAL_EVENT", typeof(IntervalEvent<ItemStructure>) },
            { "POINT_EVENT", typeof(PointEvent<ItemStructure> ) },
        };
    }
}
