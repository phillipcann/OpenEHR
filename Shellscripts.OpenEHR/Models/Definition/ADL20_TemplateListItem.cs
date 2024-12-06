namespace Shellscripts.OpenEHR.Models.Definition
{
    public class ADL20_TemplateListItem
    {
        public string Id { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Concept { get; set; } = string.Empty;
        public string ArchetypeId { get; set; } = string.Empty;
        public DateTimeOffset? Created { get; set; } = null;
    }
}
