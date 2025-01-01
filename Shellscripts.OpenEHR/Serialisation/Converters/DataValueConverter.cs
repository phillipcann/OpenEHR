namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    public class DataValueConverter : EhrItemJsonConverter<DataValue>
    {
        public DataValueConverter(ILogger<DataValueConverter> logger) 
            : base(logger) { }

        // TODO : This list might not be exhaustive
        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {
            { "DATA_VALUE", typeof(DataValue) },
            { "DV_BOOLEAN", typeof(DvBoolean) },
            { "DV_STATE", typeof(DvState) },
            { "DV_IDENTIFIER", typeof(DvIdentifier) },
            { "DV_TEXT", typeof(DvText) },
            { "DV_PARAGRAPH", typeof(DvParagraph) },
            { "DV_ORDERED", typeof(DvOrdered) },
            { "DV_INTERVAL", typeof(DvInterval) },
            { "DV_TIME_SPECIFICATION", typeof(DvTimeSpecification) },
            { "DV_ENCAPSULATED", typeof(DvEncapsulated) },
            { "DV_URI", typeof(DvUri) },
            { "DV_CODED_TEXT", typeof(DvCodedText) },
            { "DV_QUANTITY", typeof(DvQuantity) },
            { "DV_ORDINAL", typeof(DvOrdinal) },
            { "DV_COUNT", typeof(DvCount) },
            { "DV_PROPORTION", typeof(DvProportion) },
        };        
    }    
}
