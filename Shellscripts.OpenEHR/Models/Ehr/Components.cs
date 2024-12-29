namespace Shellscripts.OpenEHR.Models.Ehr
{
    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Models.DataTypes;


    #region EHR Information Model (https://specifications.openehr.org/releases/RM/latest/ehr.html#_ehr_information_model)

    #region 4.8 - https://specifications.openehr.org/releases/RM/latest/ehr.html#_class_descriptions

    public class Ehr
    {
        [JsonPropertyName("system_id")]
        public HierObjectId SystemId { get; set; }

        [JsonPropertyName("ehr_id")]
        public HierObjectId EhrId { get; set; }

        [JsonPropertyName("contributions")]
        public ObjectRef[] Contributions { get; set; }

        [JsonPropertyName("ehr_status")]
        public EhrStatus EhrStatus { get; set; }

        [JsonPropertyName("ehr_access")]
        public EhrAccess EhrAccess { get; set; }

        [JsonPropertyName("compositions")]
        public ObjectRef[] Compositions { get; set; }

        [JsonPropertyName("directory")]
        public ObjectRef Directory { get; set; }

        [JsonPropertyName("time_created")]
        public DvDateTime TimeCreated { get; set; }

        [JsonPropertyName("folders")]
        public ObjectRef[] Folders { get; set; }
    }

    public class VersionedEhrAccess : VersionedObject { }

    /// <summary>
    /// EhrAccess
    /// </summary>
    /// <remarks>https://specifications.openehr.org/releases/RM/latest/ehr.html#_ehr_access_class</remarks>
    public class EhrAccess : Locatable
    {

        // TODO : Missing ACCESS_CONTROL_SETTINGS Class
        [JsonPropertyName("settings")]
        public object Settings { get; set; }
    }

    public class VersionedEhrStatus : VersionedObject { }

    public class EhrStatus : Locatable
    {
        [JsonPropertyName("subject")]
        public PartySelf Subject { get; set; }

        [JsonPropertyName("is_queryable")]
        public bool IsQueryable { get; set; }

        [JsonPropertyName("is_modifiable")]
        public bool IsModifiable { get; set; }

        [JsonPropertyName("other_details")]
        public ItemStructure OtherDetails { get; set; }

    }

    public class VersionedComposition : VersionedObject
    {

    }


    #endregion


    #region 5.4 - https://specifications.openehr.org/releases/RM/latest/ehr.html#_class_descriptions_2

    public class Composition : Locatable
    {
        [JsonPropertyName("language")]
        public CodePhrase Language { get; set; }

        [JsonPropertyName("territory")]
        public CodePhrase Territory { get; set; }

        [JsonPropertyName("category")]
        public DvCodedText Category { get; set; }

        [JsonPropertyName("context")]
        public EventContext Context { get; set; }

        [JsonPropertyName("composer")]
        public PartyProxy Composer { get; set; }

        [JsonPropertyName("content")]
        public ContentItem[] Content { get; set; }

    }

    public class EventContext : Pathable
    {
        [JsonPropertyName("start_time")]
        public DvDateTime StartTime { get; set; }

        [JsonPropertyName("end_time")]
        public DvDateTime EndTime { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("setting")]
        public DvCodedText Setting { get; set; }

        [JsonPropertyName("other_context")]
        public ItemStructure OtherContext { get; set; }

        [JsonPropertyName("health_care_facility")]
        public PartyIdentified HealthCareFacility { get; set; }

        [JsonPropertyName("participations")]
        public Participation[] Participations { get; set; }

    }

    #endregion


    #region 6.2 - https://specifications.openehr.org/releases/RM/latest/ehr.html#_class_descriptions_3

    public class ContentItem : Locatable
    { }


    #endregion


    #region 7.2 - https://specifications.openehr.org/releases/RM/latest/ehr.html#_section_class

    public class Section : ContentItem
    {
        [JsonPropertyName("items")]
        public ContentItem[] Items { get; set; }
    }

    #endregion



    #region 8.3 - https://specifications.openehr.org/releases/RM/latest/ehr.html#_class_descriptions_5

    public class Entry : ContentItem
    {
        [JsonPropertyName("language")]
        public CodePhrase Language { get; set; }

        [JsonPropertyName("encoding")]
        public CodePhrase Encoding { get; set; }

        [JsonPropertyName("other_participations")]
        public Participation[] OtherParticipations { get; set; }

        [JsonPropertyName("workflow_id")]
        public ObjectRef WorkflowId { get; set; }

        [JsonPropertyName("subject")]
        public PartyProxy Subject { get; set; }

        [JsonPropertyName("provider")]
        public PartyProxy Provider { get; set; }
    }

    public class AdminEntry : Entry
    {
        [JsonPropertyName("data")]
        public ItemStructure Data { get; set; }
    }

    public class CareEntry : Entry 
    {
        [JsonPropertyName("protocol")]
        public ItemStructure Protocol { get; set; }

        [JsonPropertyName("guideline_id")]
        public ObjectRef GuidelineId { get; set; }
    }

    public class Observation : CareEntry
    {
        [JsonPropertyName("data")]
        public History<ItemStructure> Data { get; set; }

        [JsonPropertyName("state")]
        public History<ItemStructure> State { get; set; }
    }

    public class Evaluation : CareEntry { }

    public class Instruction : CareEntry
    {
        [JsonPropertyName("narrative")]
        public DvText Narrative { get; set; }

        [JsonPropertyName("expiry_time")]
        public DvDateTime ExpiryTime { get; set; }

        [JsonPropertyName("wf_definition")]
        public DvParsable WorkflowDefinition { get; set; }

        [JsonPropertyName("activities")]
        public Activity[] Activities { get; set; }

    }

    public class Activity : Locatable 
    {
        [JsonPropertyName("timing")]
        public DvParsable Timing { get; set; }

        [JsonPropertyName("action_archetype_id")]
        public string ActionArchetypeId { get; set; }

        [JsonPropertyName("description")]
        public ItemStructure Description { get; set; }
    }

    public class Action : CareEntry
    {
        [JsonPropertyName("time")]
        public DvDateTime Time { get; set; }

        [JsonPropertyName("ism_transition")]
        public IsmTransition IsmTransition { get; set; }

        [JsonPropertyName("instruction_details")]
        public InstructionDetails InstructionDetails { get; set; }

        [JsonPropertyName("description")]
        public ItemStructure Description { get; set; }
    }

    public class InstructionDetails : Pathable 
    {
        [JsonPropertyName("instruction_id")]
        public LocatableRef InstructionId { get; set; }

        [JsonPropertyName("activity_id")]
        public string ActivityId { get; set; }

        [JsonPropertyName("wf_details")]
        public ItemStructure WorkflowDetails { get; set; }


    }

    public class IsmTransition : Pathable 
    {
        [JsonPropertyName("current_state")]
        public DvCodedText CurrentState { get; set; }

        [JsonPropertyName("transition")]
        public DvCodedText Transition { get; set; }

        [JsonPropertyName("careflow_step")]
        public DvCodedText CareFlowStep { get; set; }

        [JsonPropertyName("reason")]
        public DvText[] Reason { get; set; }


    }

    #endregion

    #endregion

}
