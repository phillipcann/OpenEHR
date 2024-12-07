namespace Shellscripts.OpenEHR.Models.Components
{
    using System.Security;
    using System.Text.Json.Serialization;
    using Real = Double;

    public class Name
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }



    public class Uid
    {
        [JsonPropertyName("_type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class IsoOid : Uid { }

    public class Uuid : Uid { }

    public class InternetId : Uid { }

    public class ObjectId
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class UidBasedId : ObjectId
    {
        [JsonPropertyName("root")]
        public Uid Root { get; set; }

        [JsonPropertyName("extension")]
        public string Extension { get; set; }

        public bool HasExtension => !string.IsNullOrWhiteSpace(Extension);
    }

    public class HierObjectId : UidBasedId { }

    public class ObjectVersionId : UidBasedId
    {
        [JsonPropertyName("object_id")]
        public Uid ObjectId { get; set; }

        [JsonPropertyName("creating_system_id")]
        public Uid CreatingSystemId { get; set; }

        [JsonPropertyName("version_tree_id")]
        public VersionTreeId VersionTreeId { get; set; }

        [JsonPropertyName("is_branch")]
        public bool IsBranch { get; set; }
    }

    public class VersionTreeId
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("trunk_version")]
        public string TrunkVersion { get; set; }

        [JsonPropertyName("branch_number")]
        public string BranchNumber { get; set; }

        [JsonPropertyName("branch_version")]
        public string BranchVersion { get; set; }
    }

    public class ArchetypeId : ObjectId
    {
        [JsonPropertyName("qualified_rm_entity")]
        public string QualifiedRmEntity { get; set; }

        [JsonPropertyName("domain_concept")]
        public string DomainConcept { get; set; }

        [JsonPropertyName("rm_originator")]
        public string RmOriginator { get; set; }

        [JsonPropertyName("rm_name")]
        public string RmName { get; set; }

        [JsonPropertyName("rm_entity")]
        public string RmEntity { get; set; }

        [JsonPropertyName("specialisation")]
        public string Specialisation { get; set; }

        [JsonPropertyName("version_id")]
        public string VersionId { get; set; }

    }

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
        public string Scheme { get; set; }
    }

    public class ObjectRef
    {
        [JsonPropertyName("namespace")]
        public string Namespace { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public ObjectId Id { get; set; }
    }

    public class PartyRef : ObjectRef
    { }

    public class LocateableRef : ObjectRef
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        [JsonPropertyName("id")]
        public UidBasedId Id { get; set; }

        public string AsUri => $"{this.Namespace}{this.Path}{this.Id}";
    }

    #region 4.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions

    public class DataValue { }
    public class DvBoolean : DataValue 
    {
        [JsonPropertyName("value")]
        public bool Value { get; set; }
    }
    public class DvState : DataValue 
    {
        [JsonPropertyName("value")]
        public DvCodedText Value { get; set; }

        [JsonPropertyName("is_terminal")]
        public bool IsTerminal { get; set; }
    }
    public class DvIdentifier : DataValue
    {
        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("assigner")]
        public string Assigner { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    #endregion

    #region 5.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_2

    public class DvText : DataValue 
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("hyperlink")]
        public DvUri Hyperlink { get; set; }

        [JsonPropertyName("formatting")]
        public string Formatting { get; set; }

        [JsonPropertyName("mappings")]
        public TermMapping[] Mappings { get; set; }

        [JsonPropertyName("language")]
        public CodePhrase Language { get; set; }

        [JsonPropertyName("encoding")]
        public CodePhrase Encoding { get; set; }

    }
    
    public class TermMapping 
    {
        [JsonPropertyName("match")]
        public char Match { get; set; }

        [JsonPropertyName("purpose")]
        public DvCodedText Purpose { get; set; }
        
        [JsonPropertyName("target")]
        public CodePhrase Target { get; set; }
    }
    
    public class CodePhrase 
    {
        [JsonPropertyName("terminology_id")]
        public TerminologyId TerminologyId { get; set; }

        [JsonPropertyName("code_string")]
        public string CodeString { get; set; }

        [JsonPropertyName("preferred_term")]
        public string PreferredTerm { get; set; }
    }
    
    public class DvCodedText : DvText 
    {
        [JsonPropertyName("defining_code")]
        public CodePhrase DefiningCode { get; set; }
    }
    
    public class DvParagraph : DataValue 
    {
        [JsonPropertyName("items")]
        public DvText[] Items { get; set; }
    }

    #endregion


    public class DvUri : DataValue 
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class DvEhrUri : DvUri { }

    #region 7.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_4

    public class DvTemporal : DvAbsoluteQuantity
    {
        [JsonPropertyName("accuracy")]
        public DvDuration Accuracy { get; set; }
    }

    public class DvDate : DvTemporal 
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class DvTime : DvTemporal 
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class DvDateTime : DvTemporal 
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

    }

    public class DvDuration : DvAmount 
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    #endregion

    #region 6.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_3

    public class DvOrdered : DataValue 
    {
        [JsonPropertyName("normal_status")]
        public CodePhrase NormalStatus { get; set; }

        [JsonPropertyName("normal_range")]
        public DvInterval NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange[] OtherReferenceRanges { get; set; }
    }

    public class DvInterval : DataValue
    { 
        
    }

    public class ReferenceRange 
    {
        [JsonPropertyName("meaning")]
        public DvText Meaning { get; set; }

        [JsonPropertyName("range")]
        public DvInterval Range { get; set; }
    }

    public class DvOrdinal : DvOrdered
    {
        [JsonPropertyName("symbol")]
        public DvText Symbol { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    public class DvScale : DvOrdered
    {
        [JsonPropertyName("symbol")]
        public DvCodedText Symbol { get; set; }

        [JsonPropertyName("value")]
        public double Value { get; set; }
    }

    public class DvQuantified : DvOrdered 
    {
        [JsonPropertyName("magnitude_status")]
        public string MagnitudeStatus { get; set; }

        [JsonPropertyName("accuracy")]
        public object Accuracy { get; set; }
    }

    public class DvAmount : DvQuantified 
    {
        [JsonPropertyName("accuracy_is_percent")]
        public bool AccuracyIsPercent { get; set; }

        [JsonPropertyName("accuracy")]
        public double Accuracy { get; set; }
    }

    public class DvQuantity : DvAmount 
    {
        [JsonPropertyName("magnitude")]
        public double Magnitude { get; set; }

        [JsonPropertyName("precision")]
        public int Precision { get; set; }

        [JsonPropertyName("units")]
        public string Units { get; set; }

        [JsonPropertyName("units_system")]
        public string UnitsSystem { get; set; }

        [JsonPropertyName("units_display_name")]
        public string UnitsDisplayName { get; set; }


        // TODO : Ascertain best approach for defining this property
        [JsonPropertyName("normal_range")]
        public object NormalRange { get; set; }

        // TODO : Ascertain best approach for defining this property
        [JsonPropertyName("other_reference_ranges")]
        public object OtherReferenceRanges { get; set; }

    }

    public class DvCount : DvAmount 
    {
        [JsonPropertyName("magnitude")]
        public Int64? Magnitude { get; set; }

        // TODO : Ascertain best approach for defining this property
        [JsonPropertyName("normal_range")]
        public object NormalRange { get; set; }

        // TODO : Ascertain best approach for defining this property
        [JsonPropertyName("other_reference_ranges")]
        public object OtherReferenceRanges { get; set; }
    }

    public class DvProportion : DvAmount 
    {
        [JsonPropertyName("numerator")]
        public double Numerator { get; set; }

        [JsonPropertyName("denominator")]
        public double Denominator { get; set; }

        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("precision")]
        public int Precision { get; set; }

        // TODO : Ascertain best approach for defining this property
        [JsonPropertyName("normal_range")]
        public object NormalRange { get; set; }

        // TODO : Ascertain best approach for defining this property
        [JsonPropertyName("other_reference_ranges")]
        public object OtherReferenceRanges { get; set; }

    }

    public class ProportionKind { }

    public class DvAbsoluteQuantity { }

    #endregion


    public class VersionedObject
    {
        [JsonPropertyName("uid")]
        public HierObjectId Uid { get; set; }

        [JsonPropertyName("owner_id")]
        public ObjectRef OwnerId { get; set; }

        [JsonPropertyName("time_created")]
        public DvDateTime TimeCreated { get; set; }

    }


    #region 3.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/common.html#_class_definitions

    public class Pathable
    {

    }

    public class Locateable : Pathable
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

    #region 9.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_6

    public class DvEncapsulated : DataValue 
    {
        [JsonPropertyName("charset")]
        public CodePhrase Charset { get; set; }

        [JsonPropertyName("language")]
        public CodePhrase Language { get; set; }

    }

    public class DvMultiMedia : DvEncapsulated 
    {
        [JsonPropertyName("alternate_text")]
        public string AlternateText { get; set; }

        [JsonPropertyName("uri")]
        public DvUri Uri { get; set; }

        [JsonPropertyName("data")]
        public byte[] Data { get; set; }

        [JsonPropertyName("media_type")]
        public CodePhrase MediaType { get; set; }

        [JsonPropertyName("compression_algorithm")]
        public CodePhrase CompressionAlgorithm { get; set; }

        [JsonPropertyName("integrity_check")]
        public byte[] IntegrityCheck { get; set; }

        [JsonPropertyName("integrity_check_algorithm")]
        public CodePhrase IntegrityCheckAlgorithm { get; set; }

        [JsonPropertyName("thumbnail")]
        public DvMultiMedia Thumbnail { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }
    }

    public class DbParsable : DvEncapsulated
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("formalism")]
        public string Formalism { get; set; }
    }

    #endregion

}
