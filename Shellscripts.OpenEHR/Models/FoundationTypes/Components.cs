namespace Shellscripts.OpenEHR.Models.FoundationTypes
{
    using System.Text.Json.Serialization;

    // TODO : This set of classes has very little implementation and no unit testing associated

    // TypeCrossTable : https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_type_cross_reference


    #region Foundation Types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html


    #region 3.3 - Primitive Types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_primitive_types

    // Boolean : Any

    public abstract class Ordered
    {

    }

    // Character : Ordered
    // Octet : Ordered
    // String : Ordered
    // Uri : String
    // abstract Numeric : Any
    // OrderedNumeric : Numeric, Ordered
    // Integer : OrderedNumeric
    // Integer64 : OrderedNumeric
    // Real : OrderedNumeric
    // Double : OrderedNumeric

    #endregion


    #region 4.2 - Structure Types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_class_definitions_3

    // abstract Container<T> : Any
    // List<T> : Container<T>
    // Set<T> : Container<T>
    // Array<T> : Container<T>
    // Hash<K, V> : Container<T> -> This is odd.... should be a keyvaluepair collection

    #endregion


    #region 5.2 - Interval - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_class_definitions_4

    /// <summary>
    /// Technical should be an abstract class but c# doesn't allow for multiple inheritence so this can be achieved via interfaces
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInterval<T>
    {
        public T? Lower { get; set; }
        public T? Upper { get; set; }
        public Boolean? LowerUnbounded { get; set; }
        public Boolean? UpperUnbounded { get; set; }
        public Boolean? LowerIncluded { get; set; }
        public Boolean? UpperIncluded { get; set; }
    }

    // PointInterval<T> : Interval<T>
    // ProperInterval<T> : Interval<T>
    // MultiplicityInterval<T> : ProperInterval<T>
    // Cardinality ???


    #endregion


    #region 6.5 - Time Types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_time_types

    public static class TimeDefinitions
    {
        public static int SecondsInMinute = 60;
        
        public static int MinutesInHour = 60;
        
        public static int HoursInDay = 24;
        
        /// <summary>
        /// Used for conversions of durations containing motnsh to days and / or seconds
        /// </summary>
        public static double AverageDaysInMonth = 30.42;
        
        public static int MaxDaysInMonth = 31;
        
        /// <summary>
        /// Calendar days in a normal year, i.e. 365
        /// </summary>
        public static int DaysInYear = 365;

        /// <summary>
        /// Used for conversions of durations containing years to days and / or seconds
        /// </summary>
        public static double AverageDaysInYear = 365.24;

        public static int DaysInLeapYear = 366;

        /// <summary>
        /// Maximum number of days in a year, i.e. accounting for leap years.
        /// </summary>
        // TODO : This value is actually missing from the openehr specifications. I have filled it in here
        public static int MaxDaysInYear = 366;

        public static int DaysInWeek = 7;

        public static int MonthsInYear = 12;

        /// <summary>
        /// Minimum hour value of a timezone according to ISO8601 (not that -ve sign is supplied in the ISO8601_TIMEEZONE class
        /// </summary>
        public static int MinTimezoneHour = 12;

        /// <summary>
        /// Maximum hour value accoding to ISO8601.
        /// </summary>
        public static int MaxTimezoneHour = 14;

        public static double NominalDaysInMonth = 30.42;

        public static double NominalDaysInYear = 365.24;
    }

    public abstract class Temporal : Ordered
    {

    }

    public abstract class ISO8601Type : Temporal
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
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


    #region 7.2 - Terminology Package - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_class_definitions_6

    public class TerminologyTerm
    {

        [JsonPropertyName("concept")]
        public TerminologyCode? Concept { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

    public class TerminologyCode
    {
        [JsonPropertyName("terminology_id")]
        public string? TerminologyId { get; set; }

        [JsonPropertyName("terminology_version")]
        public string? TerminologyVersion { get; set; }

        [JsonPropertyName("code_string")]
        public string? CodeString { get; set; }

        // TODO : This type should reflect a 'Uri' foundation type. Not yet defined.
        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }


    #endregion


    #region 8.1 - Functional Meta-types - https://specifications.openehr.org/releases/BASE/latest/foundation_types.html#_class_definitions_7

    // Routine
    // Function : Routine
    // Procedure : Routine
    // Tuple
    // Tuple1 : Tuple
    // Tuple2 : Tuple


    #endregion

    #endregion

}
