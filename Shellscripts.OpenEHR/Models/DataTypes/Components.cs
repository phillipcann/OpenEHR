namespace Shellscripts.OpenEHR.Models.DataTypes
{
    // Data Types Information Model (https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_data_types_information_model)

    using System;
    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Attribution;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.FoundationTypes;

    #region 4.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions

    [TypeMap("DATA_VALUE")]
    public abstract class DataValue { }

    [TypeMap("DV_BOOLEAN")]
    public class DvBoolean : DataValue
    {
        [JsonPropertyName("value")]
        public bool? Value { get; set; }
    }

    [TypeMap("DV_STATE")]
    public class DvState : DataValue
    {
        [JsonPropertyName("value")]
        public DvCodedText? Value { get; set; }

        [JsonPropertyName("is_terminal")]
        public bool? IsTerminal { get; set; }
    }

    [TypeMap("DV_IDENTIFIER")]
    public class DvIdentifier : DataValue
    {
        [JsonPropertyName("issuer")]
        public string? Issuer { get; set; }

        [JsonPropertyName("assigner")]
        public string? Assigner { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }

    #endregion


    #region 5.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_2

    [TypeMap("DV_TEXT")]
    public class DvText : DataValue
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("hyperlink")]
        public DvUri? Hyperlink { get; set; }

        [JsonPropertyName("formatting")]
        public string? Formatting { get; set; }

        [JsonPropertyName("mappings")]
        public TermMapping[]? Mappings { get; set; }

        [JsonPropertyName("language")]
        public CodePhrase? Language { get; set; }

        [JsonPropertyName("encoding")]
        public CodePhrase? Encoding { get; set; }

    }

    [TypeMap("TERM_MAPPING")]
    public class TermMapping
    {
        [JsonPropertyName("match")]
        public char? Match { get; set; }

        [JsonPropertyName("purpose")]
        public DvCodedText? Purpose { get; set; }

        [JsonPropertyName("target")]
        public CodePhrase? Target { get; set; }
    }

    [TypeMap("CODE_PHRASE")]
    public class CodePhrase
    {
        [JsonPropertyName("terminology_id")]
        public TerminologyId? TerminologyId { get; set; }

        [JsonPropertyName("code_string")]
        public string? CodeString { get; set; }

        [JsonPropertyName("preferred_term")]
        public string? PreferredTerm { get; set; }
    }

    [TypeMap("DV_CODED_TEXT")]
    public class DvCodedText : DvText
    {
        [JsonPropertyName("defining_code")]
        public CodePhrase? DefiningCode { get; set; }
    }

    [TypeMap("DV_PARAGRAPH")]
    public class DvParagraph : DataValue
    {
        [JsonPropertyName("items")]
        public DvText[]? Items { get; set; }
    }

    #endregion


    #region 6.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_3
    
    [TypeMap("DV_ORDERED")]
    public abstract class DvOrdered : DataValue
    {
        [JsonPropertyName("normal_status")]
        public CodePhrase? NormalStatus { get; set; }

        [JsonPropertyName("normal_range")]
        public DvInterval? NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange[]? OtherReferenceRanges { get; set; }
    }

    [TypeMap("DV_INTERVAL")]
    public class DvInterval : DataValue, IInterval<DvOrdered>
    {
        [JsonPropertyName("lower")]
        public DvOrdered? Lower { get; set; }

        [JsonPropertyName("upper")]
        public DvOrdered? Upper { get; set; }

        [JsonPropertyName("lower_unbounded")]
        public bool? LowerUnbounded { get; set; }

        [JsonPropertyName("upper_unbounded")]
        public bool? UpperUnbounded { get; set; }

        [JsonPropertyName("lower_included")]
        public bool? LowerIncluded { get; set; }

        [JsonPropertyName("upper_included")]
        public bool? UpperIncluded { get; set; }

    }

    public class DvInterval<T> : DvInterval
        where T : DvOrdered
    { }

    [TypeMap("REFERENCE_RANGE")]
    public class ReferenceRange
    {
        [JsonPropertyName("meaning")]
        public DvText? Meaning { get; set; }

        [JsonPropertyName("range")]
        public DvInterval? Range { get; set; }
    }

    // TODO : We need to check this will serialise / deserialise    
    public class ReferenceRange<T> : ReferenceRange
        where T : DvOrdered
    { }

    [TypeMap("DV_ORDINAL")]
    public class DvOrdinal : DvOrdered
    {
        [JsonPropertyName("symbol")]
        public DvCodedText? Symbol { get; set; }

        [JsonPropertyName("value")]
        public int? Value { get; set; }
    }

    [TypeMap("DV_SCALE")]
    public class DvScale : DvOrdered
    {
        [JsonPropertyName("symbol")]
        public DvCodedText? Symbol { get; set; }

        [JsonPropertyName("value")]
        public double? Value { get; set; }
    }

    [TypeMap("DV_QUANTIFIED")]
    public abstract class DvQuantified : DvOrdered
    {
        [JsonPropertyName("magnitude_status")]
        public string? MagnitudeStatus { get; set; }

        [JsonPropertyName("accuracy")]
        public object? Accuracy { get; set; }
    }

    [TypeMap("DV_AMOUNT")]
    public abstract class DvAmount : DvQuantified
    {
        [JsonPropertyName("accuracy_is_percent")]
        public bool? AccuracyIsPercent { get; set; }

        [JsonPropertyName("accuracy")]
        public double? Accuracy { get; set; }
    }

    [TypeMap("DV_QUANTITY")]
    public class DvQuantity : DvAmount
    {
        [JsonPropertyName("magnitude")]
        public double? Magnitude { get; set; }

        [JsonPropertyName("precision")]
        public int? Precision { get; set; }

        [JsonPropertyName("units")]
        public string? Units { get; set; }

        [JsonPropertyName("units_system")]
        public string UnitsSystem { get; set; }

        [JsonPropertyName("units_display_name")]
        public string? UnitsDisplayName { get; set; }

        [JsonPropertyName("normal_range")]
        public DvInterval<DvQuantity>? NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange<DvQuantity>[]? OtherReferenceRanges { get; set; }

    }

    [TypeMap("DV_COUNT")]
    public class DvCount : DvAmount
    {
        [JsonPropertyName("magnitude")]
        public Int64? Magnitude { get; set; }

        [JsonPropertyName("normal_range")]
        public DvInterval<DvCount>? NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange<DvCount>[]? OtherReferenceRanges { get; set; }
    }

    [TypeMap("DV_PROPORTION")]
    public class DvProportion : DvAmount
    {
        [JsonPropertyName("numerator")]
        public double? Numerator { get; set; }

        [JsonPropertyName("denominator")]
        public double? Denominator { get; set; }

        [JsonPropertyName("type")]
        public int? Type { get; set; }

        [JsonPropertyName("precision")]
        public int? Precision { get; set; }

        [JsonPropertyName("normal_range")]
        public DvInterval<DvProportion>? NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange<DvProportion>[]? OtherReferenceRanges { get; set; }

    }

    [TypeMap("PROPORTION_KIND")]
    public class ProportionKind { }

    [TypeMap("DV_ABSOLUTE_QUANTITY")]
    public class DvAbsoluteQuantity : DvQuantified
    {
        [JsonPropertyName("accuracy")]
        public new DvAmount? Accuracy { get; set; }
    }

    #endregion


    #region 7.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_4

    [TypeMap("DV_TEMPORAL")]
    public abstract class DvTemporal : DvAbsoluteQuantity
    {
        [JsonPropertyName("accuracy")]
        public new DvDuration? Accuracy { get; set; }
    }

    [TypeMap("DV_DATE")]
    public class DvDate : DvTemporal
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    [TypeMap("DV_TIME")]
    public class DvTime : DvTemporal
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    [TypeMap("DV_DATE_TIME")]
    public class DvDateTime : DvTemporal
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

    }

    [TypeMap("DV_DURATION")]
    public class DvDuration : DvAmount
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    #endregion


    #region 8.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_5

    // TODO : This should be an abstract class
    [TypeMap("DV_TIME_SPECIFICATION")]
    public class DvTimeSpecification : DataValue
    {
        [JsonPropertyName("value")]
        public DvParsable? Value { get; set; }

    }

    [TypeMap("DV_PERIODIC_TIME_SPECIFICATION")]
    public class DvPeriodicTimeSpecification : DvTimeSpecification
    {

    }

    [TypeMap("DV_GENERAL_TIME_SPECIFICATION")]
    public class DvGeneralTimeSpecification : DvTimeSpecification
    {

    }

    #endregion


    #region 9.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_6

    // TODO : This should be an abstract class
    [TypeMap("DV_ENCAPSULATED")]
    public class DvEncapsulated : DataValue
    {
        [JsonPropertyName("charset")]
        public CodePhrase? Charset { get; set; }

        [JsonPropertyName("language")]
        public CodePhrase? Language { get; set; }

    }

    [TypeMap("DV_MULTIMEDIA")]
    public class DvMultiMedia : DvEncapsulated
    {
        [JsonPropertyName("alternate_text")]
        public string? AlternateText { get; set; }

        [JsonPropertyName("uri")]
        public DvUri? Uri { get; set; }

        [JsonPropertyName("data")]
        public byte[]? Data { get; set; }

        [JsonPropertyName("media_type")]
        public CodePhrase? MediaType { get; set; }

        [JsonPropertyName("compression_algorithm")]
        public CodePhrase? CompressionAlgorithm { get; set; }

        [JsonPropertyName("integrity_check")]
        public byte[]? IntegrityCheck { get; set; }

        [JsonPropertyName("integrity_check_algorithm")]
        public CodePhrase? IntegrityCheckAlgorithm { get; set; }

        [JsonPropertyName("thumbnail")]
        public DvMultiMedia? Thumbnail { get; set; }

        [JsonPropertyName("size")]
        public int? Size { get; set; }
    }

    [TypeMap("DV_PARSABLE")]
    public class DvParsable : DvEncapsulated
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonPropertyName("formalism")]
        public string? Formalism { get; set; }
    }

    #endregion


    #region 10.3 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_7

    [TypeMap("DV_URI")]
    public class DvUri : DataValue
    {
        [JsonPropertyName("value")]
        public string? Value { get; set; }
    }

    [TypeMap("DV_EHR_URI")]
    public class DvEhrUri : DvUri { }

    #endregion

}
