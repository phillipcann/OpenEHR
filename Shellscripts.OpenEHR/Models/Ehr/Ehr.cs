namespace Shellscripts.OpenEHR.Models.Ehr
{
    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Models.Components;

    public class Ehr
    {
        [JsonPropertyName("system_id")]
        public HierObjectId SystemId { get; set; }

        [JsonPropertyName("ehr_id")]
        public HierObjectId EhrId { get; set; }

        [JsonPropertyName("contributions")]
        public ObjectRef[] Contributions { get; set; }

        [JsonPropertyName("ehr_status")]
        public ObjectRef EhrStatus { get; set; }

        [JsonPropertyName("ehr_access")]
        public ObjectRef EhrAccess { get; set; }

        [JsonPropertyName("compositions")]
        public ObjectRef[] Compositions { get; set; }

        [JsonPropertyName("directory")]
        public ObjectRef Directory { get; set; }

        [JsonPropertyName("time_created")]
        public DvDateTime TimeCreated { get; set; }

        [JsonPropertyName("folders")]
        public ObjectRef[] Folders { get; set; }


    }

}
