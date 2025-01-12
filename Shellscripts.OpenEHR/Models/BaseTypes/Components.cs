namespace Shellscripts.OpenEHR.Models.BaseTypes
{
    // Base Types (https://specifications.openehr.org/releases/BASE/latest/base_types.html#_base_types)

    using System.Text.Json.Serialization;

    #region 5.4 - https://specifications.openehr.org/releases/BASE/latest/base_types.html#_class_descriptions

    public abstract class Uid
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class IsoOid : Uid { }

    public class Uuid : Uid { }

    public class InternetId : Uid { }

    public class ObjectId
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class UidBasedId : ObjectId { }

    public class HierObjectId : UidBasedId { }

    public class ObjectVersionId : UidBasedId { }

    public class VersionTreeId
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    public class ArchetypeId : ObjectId { }

    public class TemplateId : ObjectId { }

    public class TerminologyId : ObjectId
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version_id")]
        public string VersionId { get; set; }
    }

    public class GenericId : ObjectId
    {
        [JsonPropertyName("scheme")]
        public string? Scheme { get; set; }
    }

    public class ObjectRef
    {
        [JsonPropertyName("namespace")]
        public string? Namespace { get; set; }

        [JsonPropertyName("type")]        
        public string? Type { get; set; }

        [JsonPropertyName("id")]
        public ObjectId? Id { get; set; }
    }

    public class PartyRef : ObjectRef
    { }

    public class LocatableRef : ObjectRef
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("id")]
        public UidBasedId? Id { get; set; }
    }


    #endregion
}
