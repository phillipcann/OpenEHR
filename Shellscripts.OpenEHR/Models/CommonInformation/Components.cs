namespace Shellscripts.OpenEHR.Models.CommonInformation
{
    // Common Information Model (https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_common_information_model)

    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Attribution;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Models.DataTypes;

    #region 3.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_class_definitions

    [TypeMap("PATHABLE")]
    public abstract class Pathable { }

    [TypeMap("LOCATABLE")]
    public abstract class Locatable : Pathable
    {
        [JsonPropertyName("name")]
        public DvText Name { get; set; }

        [JsonPropertyName("archetype_node_id")]
        public string ArchetypeNodeId { get; set; }

        [JsonPropertyName("uid")]
        public UidBasedId? Uid { get; set; }

        [JsonPropertyName("links")]
        public Link[]? Links { get; set; }

        [JsonPropertyName("archetype_details")]
        public Archetyped? ArchetypeDetails { get; set; }

        [JsonPropertyName("feeder_audit")]
        public FeederAudit? FeederAudit { get; set; }
    }

    [TypeMap("ARCHETYPED")]
    public class Archetyped
    {
        [JsonPropertyName("archetype_id")]
        public ArchetypeId ArchetypeId { get; set; }

        [JsonPropertyName("template_id")]
        public TemplateId TemplateId { get; set; }

        [JsonPropertyName("rm_version")]
        public string RmVersion { get; set; }
    }

    [TypeMap("LINK")]
    public class Link
    {
        [JsonPropertyName("meaning")]
        public DvText Meaning { get; set; }

        [JsonPropertyName("type")]
        public DvText Type { get; set; }

        [JsonPropertyName("target")]
        public DvEhrUri Target { get; set; }
    }

    [TypeMap("FEEDER_AUDIT")]
    public class FeederAudit
    {
        [JsonPropertyName("originating_system_item_ids")]
        public DvIdentifier[]? OriginatingSystemItemIds { get; set; }

        [JsonPropertyName("feeder_system_item_ids")]
        public DvIdentifier[]? FeederSystemItemIds { get; set; }

        [JsonPropertyName("original_content")]
        public DvEncapsulated? OriginalContent { get; set; }

        [JsonPropertyName("originating_system_audit")]
        public FeederAuditDetails? OriginatingSystemAudit { get; set; }

        [JsonPropertyName("feeder_system_audit")]
        public FeederAuditDetails? FeederSystemAudit { get; set; }

    }

    [TypeMap("FEEDER_AUDIT_DETAILS")]
    public class FeederAuditDetails 
    {
        [JsonPropertyName("system_id")]
        public string? SystemId { get; set; }

        [JsonPropertyName("location")]
        public PartyIdentified? Location { get; set; }

        [JsonPropertyName("subject")]
        public PartyProxy? Subject { get; set; }

        [JsonPropertyName("provider")]
        public PartyIdentified? Provider { get; set; }

        [JsonPropertyName("time")]
        public DvDateTime? Time { get; set; }

        [JsonPropertyName("version_id")]
        public string? VersionId { get; set; }

        [JsonPropertyName("other_details")]
        public ItemStructure? OtherDetails { get; set; }
    }

    #endregion

    #region 4.3 - https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_class_descriptions
    
    [TypeMap("PARTY_PROXY")]
    public abstract class PartyProxy
    {

        [JsonPropertyName("external_ref")]
        public PartyRef? ExternalReference { get; set; }
    }

    [TypeMap("PARTY_SELF")]
    public class PartySelf : PartyProxy { }

    [TypeMap("PARTY_IDENTIFIED")]
    public class PartyIdentified : PartyProxy
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("identifiers")]
        public DvIdentifier[] Identifiers { get; set; }

    }

    [TypeMap("PARTY_RELATED")]
    public class PartyRelated : PartyIdentified
    {
        [JsonPropertyName("relationship")]
        public DvCodedText Relationship { get; set; }
    }

    [TypeMap("PARTICIPATION")]
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

    [TypeMap("AUDIT_DETAILS")]
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

    [TypeMap("ATTESTATION")]
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

    [TypeMap("REVISION_HISTORY")]
    public class RevisionHistory
    {
        [JsonPropertyName("items")]
        public RevisionHistoryItem[] Items { get; set; }
    }

    [TypeMap("REVISION_HISTORY_ITEM")]
    public class RevisionHistoryItem
    {
        [JsonPropertyName("version_id")]
        public ObjectVersionId VersionId { get; set; }

        [JsonPropertyName("audits")]
        public AuditDetails[] Audits { get; set; }
    }

    #endregion

    #region 6.5 - https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_class_descriptions_3

    [TypeMap("VERSIONED_OBJECT")]
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

    // TODO : Nothing currently implements Version. We need to check if there are use cases where we need to deserialise to a Version object
    //      : because without a class we can instantiate, this will fail
    [TypeMap("VERSION")]
    public abstract class Version
    {
        [JsonPropertyName("contribution")]
        public ObjectRef? Contribution { get; set; }

        [JsonPropertyName("signature")]
        public string? Signature { get; set; }

        [JsonPropertyName("commit_audit")]
        public AuditDetails? CommitAudit { get; set; }

    }

    public abstract class Version<T> : Version where T : class { }

    #endregion

}