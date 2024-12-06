using System.Text.Json.Serialization;

namespace Shellscripts.OpenEHR.Models.Definition
{

    public class ADL14_TemplateListItem
    {
        [JsonPropertyName("template_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("concept")]
        public string Concept { get; set; } = string.Empty;

        [JsonPropertyName("archetype_id")]
        public string ArchetypeId { get; set; } = string.Empty;

        [JsonPropertyName("created_timestamp")]
        public DateTimeOffset? Created { get; set; } = null;
    }
}
