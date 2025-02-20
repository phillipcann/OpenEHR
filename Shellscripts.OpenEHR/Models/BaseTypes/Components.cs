namespace Shellscripts.OpenEHR.Models.BaseTypes
{
    // Base Types (https://specifications.openehr.org/releases/BASE/latest/base_types.html#_base_types)

    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Attribution;

    #region 5.4 - https://specifications.openehr.org/releases/BASE/latest/base_types.html#_class_descriptions

    [TypeMap("UID")]
    public abstract class Uid
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    [TypeMap("ISO_OID")]
    public class IsoOid : Uid { }

    [TypeMap("UUID")]
    public class Uuid : Uid { }

    [TypeMap("INTERNET_ID")]
    public class InternetId : Uid { }

    [TypeMap("OBJECT_ID")]
    public abstract class ObjectId
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    [TypeMap("UID_BASED_ID")]
    public class UidBasedId : ObjectId { }

    [TypeMap("HIER_OBJECT_ID")]
    public class HierObjectId : UidBasedId { }

    [TypeMap("OBJECT_VERSION_ID")]
    public class ObjectVersionId : UidBasedId { }

    [TypeMap("VERSION_TREE_ID")]
    public class VersionTreeId
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    [TypeMap("ARCHETYPE_ID")]
    public class ArchetypeId : ObjectId { }

    [TypeMap("TEMPLATE_ID")]
    public class TemplateId : ObjectId { }

    [TypeMap("TERMINOLOGY_ID")]
    public class TerminologyId : ObjectId
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("version_id")]
        public string? VersionId { get; set; }
    }

    [TypeMap("GENERIC_ID")]
    public class GenericId : ObjectId
    {
        [JsonPropertyName("scheme")]
        public string? Scheme { get; set; }
    }

    [TypeMap("OBJECT_REF")]
    public class ObjectRef
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("type")]        
        public string? Type { get; set; }

        [JsonPropertyName("id")]
        public ObjectId? Id { get; set; }
    }

    [TypeMap("PARTY_REF")]
    public class PartyRef : ObjectRef
    { }

    [TypeMap("LOCATABLE_REF")]
    public class LocatableRef : ObjectRef
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        [JsonPropertyName("id")]
        public UidBasedId? Id { get; set; }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    }


    #endregion
}
