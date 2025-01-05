namespace Shellscripts.OpenEHR.Models.PlatformServiceModel
{
    using System.Text.Json.Serialization;

    #region OpenEHR Platform Service Model (https://specifications.openehr.org/releases/SM/latest/openehr_platform.html)

    #region 8.2.4 - Result Set - https://specifications.openehr.org/releases/SM/latest/openehr_platform.html#_result_set_class

    public class ResultSet
    {
        [JsonPropertyName("meta")]
        public ResultSetMetadata? Meta { get; set; }

        [JsonPropertyName("columns")]
        public ResultSetColumn[]? Columns { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("q")]
        public string? Query { get; set; }
        
        [JsonPropertyName("rows")]
        public ResultSetRow[] Rows { get; set; }
    }

    #endregion

    #region 8.2.5 - Result Set Column - 

    public class ResultSetColumn
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("archetype_id")]
        public string? ArchetypeId { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; } 

    }

    #endregion


    #region 8.2.6 - Result Set Row - 

    public class ResultSetRow
    {
        [JsonPropertyName("values")]
        public object[]? Values { get; set; }
    }

    #endregion

    #region n.n.n - Result Set MetaData. Only referenced on the EhrBase ResultSet Schema Page - https://docs.ehrbase.org/api/hip-ehrbase/query#tag/RESULT_SET_schema

    public class ResultSetMetadata
    {
        [JsonPropertyName("_href")]
        public string? Href { get; set; }

        [JsonPropertyName("_type")]
        public string? Type { get; set; }

        [JsonPropertyName("_schema_version")]
        public string? SchemaVersion { get; set; }

        [JsonPropertyName("_created")]
        public string? Created { get; set; }

        [JsonPropertyName("_generator")]
        public string? Generator { get; set; }

        [JsonPropertyName("_executed_aql")]
        public string? ExecutedAql { get; set; }

        [JsonPropertyName("fetch")]
        public int? Fetch { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        [JsonPropertyName("resultsize")]
        public int? ResultSize { get; set; }
    }

    #endregion

    #endregion

}