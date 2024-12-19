namespace Shellscripts.OpenEHR.Models.DataTypes
{
    // Data Types Information Model (https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_data_types_information_model)

    using System;
    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Models.BaseTypes;

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

    public class DvInterval : DataValue { }

    public class DvInterval<T> : DvInterval
        where T : DvOrdered
    { }

    public class ReferenceRange
    {
        [JsonPropertyName("meaning")]
        public DvText Meaning { get; set; }

        [JsonPropertyName("range")]
        public DvInterval Range { get; set; }
    }

    public class ReferenceRange<T> : ReferenceRange
        where T : DvOrdered
    { }

    public class DvOrdinal : DvOrdered
    {
        [JsonPropertyName("symbol")]
        public DvCodedText Symbol { get; set; }

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

        [JsonPropertyName("normal_range")]
        public DvInterval<DvQuantity> NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange<DvQuantity>[] OtherReferenceRanges { get; set; }

    }

    public class DvCount : DvAmount
    {
        [JsonPropertyName("magnitude")]
        public Int64? Magnitude { get; set; }

        [JsonPropertyName("normal_range")]
        public DvInterval<DvCount> NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange<DvCount>[] OtherReferenceRanges { get; set; }
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

        [JsonPropertyName("normal_range")]
        public DvInterval<DvProportion> NormalRange { get; set; }

        [JsonPropertyName("other_reference_ranges")]
        public ReferenceRange<DvProportion>[] OtherReferenceRanges { get; set; }

    }

    public class ProportionKind { }

    public class DvAbsoluteQuantity : DvQuantified
    {
        [JsonPropertyName("accuracy")]
        public DvAmount Accuracy { get; set; }
    }

    #endregion


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


    #region 8.2 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_5

    public class DvTimeSpecification : DataValue
    {
        [JsonPropertyName("value")]
        public DvParsable Value { get; set; }

    }

    public class DvPeriodicTimeSpecification : DvTimeSpecification
    {

    }

    public class DvGeneralTimeSpecification : DvTimeSpecification
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

    public class DvParsable : DvEncapsulated
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("formalism")]
        public string Formalism { get; set; }
    }

    #endregion


    #region 10.3 - https://specifications.openehr.org/releases/RM/Release-1.1.0/data_types.html#_class_descriptions_7

    public class DvUri : DataValue
    {
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class DvEhrUri : DvUri
    {

    }

    #endregion

}
