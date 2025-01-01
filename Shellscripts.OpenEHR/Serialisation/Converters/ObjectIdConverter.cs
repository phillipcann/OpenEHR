namespace Shellscripts.OpenEHR.Serialisation.Converters
{
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Serialisation.Converters.Base;

    /// <summary>
    /// ObjectId is listed as an Abstract Class in the Reference Model however, we cannot create a 
    /// JsonConverter for this type using an Abstract Class.
    /// </summary>
    public class ObjectIdConverter : EhrItemJsonConverter<ObjectId>
    {

        public ObjectIdConverter(ILogger<ObjectIdConverter> logger)
            : base(logger) { }

        public override IDictionary<string, Type> TypeMap => new Dictionary<string, Type>
        {
            { "OBJECT_ID", typeof(ObjectId) },
            { "UID_BASED_ID", typeof(UidBasedId)},
            { "HIER_OBJECT_ID", typeof(HierObjectId) },
            { "OBJECT_VERSION_ID", typeof(ObjectVersionId)},
            { "ARCHETYPE_ID", typeof(ArchetypeId)},
            { "TEMPLATE_ID", typeof(TemplateId)},
            { "TERMINOLOGY_ID", typeof(TerminologyId)},
            { "GENERIC_ID", typeof(GenericId) }
        };
    }
}