namespace Shellscripts.OpenEHR.Models.FoundationTypes
{
    using System.Text.Json.Serialization;


    #region Foundation Types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html


    #region 3.3 - Primitive Types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_primitive_types

    #region 3.3.2 - Ordered Class

    public abstract class Ordered
    {

    }

    #endregion


    #endregion


    #region 6 - Time Types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_time_types


    #region 6.5.2 - Temporal

    public abstract class Temporal : Ordered
    {

    }

    #endregion

    public class ISO8601Type : Temporal
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class ISO8601Date : ISO8601Type
    {

    }

    public class ISO8601Time : ISO8601Type
    {

    }

    public class ISO8601DateTime : ISO8601Type
    {

    }

    public class ISO8601Duration : ISO8601Type
    {

    }

    public class ISO8601TimeZone : ISO8601Type
    {

    }

    #endregion



    #endregion

}
