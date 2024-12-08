namespace Shellscripts.OpenEHR.Models.CommonInformation
{
    // Common Information Model (https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_common_information_model)

    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.DataTypes;

    #region 3.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_class_definitions

    public class Pathable
    {

    }

    public class Locatable : Pathable
    {
        [JsonPropertyName("name")]
        public DvText Name { get; set; }

        [JsonPropertyName("archetype_node_id")]
        public string ArchetypeNodeId { get; set; }

        [JsonPropertyName("uid")]
        public UidBasedId Uid { get; set; }

        [JsonPropertyName("links")]
        public Link[] Links { get; set; }

        [JsonPropertyName("archetype_details")]
        public Archetyped ArchetypeDetails { get; set; }
    }

    public class Archetyped
    {
        [JsonPropertyName("archetype_id")]
        public ArchetypeId ArchetypeId { get; set; }

        [JsonPropertyName("template_id")]
        public TemplateId TemplateId { get; set; }

        [JsonPropertyName("rm_version")]
        public string RmVersion { get; set; }
    }

    public class Link
    {
        [JsonPropertyName("meaning")]
        public DvText Meaning { get; set; }

        [JsonPropertyName("type")]
        public DvText Type { get; set; }

        [JsonPropertyName("target")]
        public DvEhrUri Target { get; set; }
    }

    public class FeederAudit
    {
        [JsonPropertyName("originating_system_item_ids")]
        public DvIdentifier[] OriginatingSystemItemIds { get; set; }

        [JsonPropertyName("feeder_system_item_ids")]
        public DvIdentifier[] FeederSystemItemIds { get; set; }

        [JsonPropertyName("original_content")]
        public DvEncapsulated OriginalContent { get; set; }

        [JsonPropertyName("originating_system_audit")]
        public FeederAuditDetails OriginatingSystemAudit { get; set; }

        [JsonPropertyName("feeder_system_audit")]
        public FeederAuditDetails FeederSystemAudit { get; set; }

    }

    public class FeederAuditDetails
    {

    }

    #endregion

    #region 4.3 - https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_class_descriptions

    public class PartyProxy
    {

        [JsonPropertyName("external_ref")]
        public PartyRef ExternalReference { get; set; }
    }

    public class PartySelf : PartyProxy
    {

    }

    public class PartyIdentified : PartyProxy
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("identifiers")]
        public DvIdentifier[] Identifiers { get; set; }

    }

    public class PartyRelated : PartyIdentified
    {
        [JsonPropertyName("relationship")]
        public DvCodedText Relationship { get; set; }
    }

    public class Participation
    {
        [JsonPropertyName("function")]
        public DvText Function { get; set; }

        [JsonPropertyName("mode")]
        public DvCodedText Mode { get; set; }

        [JsonPropertyName("performer")]
        public PartyProxy Performer { get; set; }

        [JsonPropertyName("time")]
        public DvInterval<DvDateTime> Time { get; set; }
    }

    public class AuditDetails
    {
        [JsonPropertyName("system_id")]
        public string SystemId { get; set; }

        [JsonPropertyName("time_committed")]
        public DvDateTime TimeCommitted { get; set; }

        [JsonPropertyName("change_type")]
        public DvCodedText ChangeType { get; set; }

        [JsonPropertyName("description")]
        public DvText Description { get; set; }

        [JsonPropertyName("committer")]
        public PartyProxy Committer { get; set; }

    }

    public class Attestation : AuditDetails
    {
        [JsonPropertyName("attested_view")]
        public DvMultiMedia AttestedView { get; set; }

        [JsonPropertyName("proof")]
        public string Proof { get; set; }

        [JsonPropertyName("items")]
        public DvEhrUri[] Items { get; set; }

        [JsonPropertyName("reason")]
        public DvText Reason { get; set; }

        [JsonPropertyName("is_pending")]
        public bool IsPending { get; set; }
    }

    public class RevisionHistory
    {
        [JsonPropertyName("items")]
        public RevisionHistoryItem[] Items { get; set; }
    }

    public class RevisionHistoryItem
    {
        [JsonPropertyName("version_id")]
        public ObjectVersionId VersionId { get; set; }

        [JsonPropertyName("audits")]
        public AuditDetails[] Audits { get; set; }
    }

    #endregion

    #region 6.5 - https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_class_descriptions_3

    public class VersionedObject
    {
        [JsonPropertyName("uid")]
        public HierObjectId Uid { get; set; }

        [JsonPropertyName("owner_id")]
        public ObjectRef OwnerId { get; set; }

        [JsonPropertyName("time_created")]
        public DvDateTime TimeCreated { get; set; }

    }
    public class VersionedObject<T> : VersionedObject where T : class { }


    // TODO : This is an abstract model of one Version within a Version Container. 
    public class Version
    {
        [JsonPropertyName("contribution")]
        public ObjectRef Contribution { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("commit_audit")]
        public AuditDetails CommitAudit { get; set; }

    }
    public class Version<T> : Version where T : class { }

    #endregion

}