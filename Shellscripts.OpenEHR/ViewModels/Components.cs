using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shellscripts.OpenEHR.ViewModels
{

    /*
        "concept": "heart_anamnesis",
        "template_id": "heart_anamnesis",
        "archetype_id": "openEHR-EHR-COMPOSITION.encounter.v1",
        "created_timestamp": "2024-08-30T21:52:09.530Z"
     */

    public class AdlTemplate_14
    {
        [JsonPropertyName("concept")]
        public string Concept { get; set; }

        [JsonPropertyName("template_id")]
        public string Id { get; set; }

        [JsonPropertyName("archetype_id")]
        public string ArchetypeId { get; set; }

        [JsonPropertyName("created_timestamp")]
        public DateTime CreatedTimestamp { get; set; }
    }

    public class AdlTemplate_20
    {

    }

}
