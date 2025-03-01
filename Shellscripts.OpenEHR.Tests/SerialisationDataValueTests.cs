namespace Shellscripts.OpenEHR.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Text.Json;
    using Xunit;
    using Xunit.Abstractions;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Tests.Context;

    public class SerialisationDataValueTests : BaseTest
    {

        public SerialisationDataValueTests(ITestOutputHelper outputHelper, TestFixture testFixture)
            : base(outputHelper, testFixture) { }

        #region 4.2

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvBoolean_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvBoolean() { Value = true };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_BOOLEAN");
            AssertJsonValueEquality(actual_json, "$.value", true);
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvState_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvState()
            {

                Value = new DvCodedText() { 
                    Value = "TextField", 
                    Formatting = "FormattingText" 
                },
                IsTerminal = true
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_STATE");
            AssertJsonValueEquality(actual_json, "$.value._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.value.value", "TextField");
            AssertJsonValueEquality(actual_json, "$.value.formatting", "FormattingText");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvIdentifier_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvIdentifier() { 
                Assigner = "Assigner",
                Id= "Id",
                Issuer = "Issuer",
                Type = "Type"                 
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_IDENTIFIER");
            AssertJsonValueEquality(actual_json, "$.assigner", "Assigner");
            AssertJsonValueEquality(actual_json, "$.id", "Id");
            AssertJsonValueEquality(actual_json, "$.issuer", "Issuer");
            AssertJsonValueEquality(actual_json, "$.type", "Type");            
        }

        #endregion

        #region 5.2

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvText_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvText() { 
                Value = "Some Text Value",
                Hyperlink = new DvUri() {  Value = "https://about:blank" },
                Formatting = "Formatting",
                Mappings = [new TermMapping() 
                { 
                    Match = 'A',
                    Purpose = new DvCodedText()
                    {
                        DefiningCode = new CodePhrase() { CodeString = "CS1"},
                        Encoding = new CodePhrase() { PreferredTerm = "PT2"},
                        Formatting = "Formatting",
                        Hyperlink = new DvUri() { Value = "https://about:blank"},
                        Language = new CodePhrase() { CodeString = "CS2" },
                        Mappings = [new TermMapping() { Match = 'N' }],
                        Value = "Value"
                    },
                    Target = new CodePhrase()
                    {
                        CodeString = "CS1",
                        PreferredTerm = "PT1",
                        TerminologyId = new TerminologyId() {
                            Name = "NameString",
                            Value = "ValueString",
                            VersionId = "1.0.1"
                        }
                    }
                }] ,
                Language = new CodePhrase() { 
                    CodeString = "CS1", 
                    PreferredTerm = "PT1", 
                    TerminologyId = new TerminologyId() {
                        Name = "T_Name1",
                        Value = "T_Value1",
                        VersionId = "1.0.1"
                    } 
                },
                Encoding = new CodePhrase() { 
                    CodeString = "CS2", 
                    PreferredTerm = "PT2",
                    TerminologyId = new TerminologyId()
                    {
                        Name = "T_Name2",
                        Value = "T_Value2",
                        VersionId = "2.0.1"
                    }
                }
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.value", "Some Text Value");
            AssertJsonValueEquality(actual_json, "$.hyperlink.value", "https://about:blank");
            AssertJsonValueEquality(actual_json, "$.formatting", "Formatting");
            AssertJsonValueEquality(actual_json, "$.mappings[0].match", "A");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.defining_code.code_string", "CS1");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.encoding.preferred_term", "PT2");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.formatting", "Formatting");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.hyperlink._type", "DV_URI");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.hyperlink.value", "https://about:blank");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.language.code_string", "CS2");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.mappings[0].match", "N");
            AssertJsonValueEquality(actual_json, "$.mappings[0].purpose.value", "Value");
            AssertJsonValueEquality(actual_json, "$.mappings[0].target.code_string", "CS1");
            AssertJsonValueEquality(actual_json, "$.mappings[0].target.preferred_term", "PT1");
            AssertJsonValueEquality(actual_json, "$.mappings[0].target.terminology_id.name", "NameString");
            AssertJsonValueEquality(actual_json, "$.mappings[0].target.terminology_id.value", "ValueString");
            AssertJsonValueEquality(actual_json, "$.mappings[0].target.terminology_id.version_id", "1.0.1");
            AssertJsonValueEquality(actual_json, "$.language.code_string", "CS1");
            AssertJsonValueEquality(actual_json, "$.language.preferred_term", "PT1");
            AssertJsonValueEquality(actual_json, "$.language.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.language.terminology_id.name", "T_Name1");
            AssertJsonValueEquality(actual_json, "$.language.terminology_id.value", "T_Value1");
            AssertJsonValueEquality(actual_json, "$.language.terminology_id.version_id", "1.0.1");
            AssertJsonValueEquality(actual_json, "$.encoding.code_string", "CS2");
            AssertJsonValueEquality(actual_json, "$.encoding.preferred_term", "PT2");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id.name", "T_Name2");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id.value", "T_Value2");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id.version_id", "2.0.1");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseTermMapping_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new TermMapping() 
            { 
                Match = 'M',
                Purpose = new DvCodedText()
                {
                    DefiningCode = new CodePhrase() {  CodeString = "CS1"},
                    Encoding = new CodePhrase() {  PreferredTerm = "PT2"},
                    Formatting = "Formatting",
                    Hyperlink = new DvUri() {  Value = "https://about:blank"},
                    Language = new CodePhrase() {  CodeString = "CS2" },
                    Mappings = [new TermMapping() { Match = 'N' }],
                    Value = "Value"
                },
                Target = new CodePhrase()
                {
                    CodeString = "CS2",
                    PreferredTerm = "PT2",
                    TerminologyId = new TerminologyId()
                    {
                        Name = "Name",
                        Value = "Value",
                        VersionId = "1.0.1"
                    }
                }
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            //AssertJsonValueEquality(actual_json, "$._type", "TERM_MAPPING");
            AssertJsonValueEquality(actual_json, "$.match", "M");
            AssertJsonValueEquality(actual_json, "$.purpose._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.purpose.defining_code.code_string", "CS1");
            AssertJsonValueEquality(actual_json, "$.purpose.encoding.preferred_term", "PT2");
            AssertJsonValueEquality(actual_json, "$.purpose.formatting", "Formatting");
            AssertJsonValueEquality(actual_json, "$.purpose.hyperlink.value", "https://about:blank");
            AssertJsonValueEquality(actual_json, "$.purpose.language.code_string", "CS2");
            AssertJsonValueEquality(actual_json, "$.purpose.mappings[0].match", "N");
            AssertJsonValueEquality(actual_json, "$.purpose.value", "Value");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseCodePhrase_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new CodePhrase() { 
                TerminologyId = new TerminologyId()
                {
                    Name = "NameString",
                    Value = "ValueString",
                    VersionId = "VersionString"
                },
                CodeString = "CodeString",
                PreferredTerm = "PreferredTermString"
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            //AssertJsonValueEquality(actual_json, "$._type", "CODE_PHRASE");
            AssertJsonValueEquality(actual_json, "$.terminology_id.name", "NameString");
            AssertJsonValueEquality(actual_json, "$.terminology_id.value", "ValueString");
            AssertJsonValueEquality(actual_json, "$.terminology_id.version_id", "VersionString");
            AssertJsonValueEquality(actual_json, "$.code_string", "CodeString");
            AssertJsonValueEquality(actual_json, "$.preferred_term", "PreferredTermString");            
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvCodedText_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvCodedText()
            {
                DefiningCode = new CodePhrase() { 
                    CodeString = "CS1", 
                    PreferredTerm = "PT1", 
                    TerminologyId = new TerminologyId()
                    {
                        Name = "Name1",
                        Value = "Value1",
                        VersionId = "1.0.1"
                    }
                },
                Encoding = new CodePhrase()
                {
                    CodeString = "CS2",
                    PreferredTerm = "PT2",
                    TerminologyId = new TerminologyId()
                    {
                        Name = "Name2",
                        Value = "Value2",
                        VersionId = "2.0.1"
                    }
                },
                Formatting = "Formatting",
                Hyperlink = new DvUri() { Value = "https://about:blank" },
                Language = null,
                Mappings = null,
                Value = null
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.defining_code.code_string", "CS1");
            AssertJsonValueEquality(actual_json, "$.defining_code.preferred_term", "PT1");
            AssertJsonValueEquality(actual_json, "$.defining_code.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.defining_code.terminology_id.name", "Name1");
            AssertJsonValueEquality(actual_json, "$.defining_code.terminology_id.value", "Value1");
            AssertJsonValueEquality(actual_json, "$.defining_code.terminology_id.version_id", "1.0.1");
            AssertJsonValueEquality(actual_json, "$.encoding.code_string", "CS2");
            AssertJsonValueEquality(actual_json, "$.encoding.preferred_term", "PT2");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id.name", "Name2");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id.value", "Value2");
            AssertJsonValueEquality(actual_json, "$.encoding.terminology_id.version_id", "2.0.1");
            AssertJsonValueEquality(actual_json, "$.formatting", "Formatting");
            AssertJsonValueEquality(actual_json, "$.hyperlink._type", "DV_URI");
            AssertJsonValueEquality(actual_json, "$.hyperlink.value", "https://about:blank");
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvParagraph_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvParagraph()
            {
                Items = [
                    new DvText() { Value = "DV1Value" },
                    new DvText() { Value = "DV2Value" },
                    new DvText() { Value = "DV3Value" },
                ]
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_PARAGRAPH");
            AssertJsonValueEquality(actual_json, "$.items[0]._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.items[0].value", "DV1Value");
            AssertJsonValueEquality(actual_json, "$.items[1]._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.items[1].value", "DV2Value");
            AssertJsonValueEquality(actual_json, "$.items[2]._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.items[2].value", "DV3Value");
        }

        #endregion

        #region 6.2

        // TODO : DvInterval has a strange definition as it required multiple inheritence. I've tried to resolve this with an Interface
        // TODO : ReferenceRange has a strange definition.


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvOrdinal_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvOrdinal()
            {
                NormalRange = new DvInterval()
                {
                    Lower = new DvOrdinal() {  Value = 1, Symbol = new DvCodedText() {  Value = "C"} },
                    Upper = new DvOrdinal() {  Value = 100, Symbol = new DvCodedText() {  Value = "C" } }
                },
                NormalStatus = new CodePhrase()
                {
                    CodeString = "CodeString",
                    PreferredTerm = "PreferredTerm",
                    TerminologyId = new TerminologyId()
                    {
                        Name = "TermName",
                        Value = "TermValue",
                        VersionId = "1.0.1"
                    }
                },
                OtherReferenceRanges = [
                    new ReferenceRange() {
                        Meaning = new DvText() { Value = "TextValue"},
                        Range = new DvInterval() {
                            Lower = new DvOrdinal() {  Value = 1, Symbol = new DvCodedText() {  Value = "C"} },
                            Upper = new DvOrdinal() {  Value = 100, Symbol = new DvCodedText() {  Value = "C" } }
                        }
                    }
                ],
                Symbol = new DvCodedText() 
                { 
                    DefiningCode = null,
                    Encoding = null,
                    Formatting = "Formatting",
                    Hyperlink = new DvUri() { Value = "https://about:blank" },
                    Language = null,
                    Mappings = null,
                    Value = "ValueString"
                },
                Value = 10
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.normal_range._type", "DV_INTERVAL");
            AssertJsonValueEquality(actual_json, "$.normal_range.lower._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.normal_range.lower.value", 1);
            AssertJsonValueEquality(actual_json, "$.normal_range.lower.symbol.value", "C");
            AssertJsonValueEquality(actual_json, "$.normal_range.upper._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.normal_range.upper.value", 100);
            AssertJsonValueEquality(actual_json, "$.normal_range.upper.symbol.value", "C");
            AssertJsonValueEquality(actual_json, "$.normal_status.code_string", "CodeString");
            AssertJsonValueEquality(actual_json, "$.normal_status.preferred_term", "PreferredTerm");
            AssertJsonValueEquality(actual_json, "$.normal_status.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.normal_status.terminology_id.name", "TermName");
            AssertJsonValueEquality(actual_json, "$.normal_status.terminology_id.value", "TermValue");
            AssertJsonValueEquality(actual_json, "$.normal_status.terminology_id.version_id", "1.0.1");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].meaning._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].meaning.value", "TextValue");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range._type", "DV_INTERVAL");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range.lower._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range.lower.value", 1);
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range.lower.symbol._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range.lower.symbol.value", "C");
            AssertJsonValueEquality(actual_json, "$.symbol._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.symbol.formatting", "Formatting");
            AssertJsonValueEquality(actual_json, "$.symbol.hyperlink._type", "DV_URI");
            AssertJsonValueEquality(actual_json, "$.symbol.hyperlink.value", "https://about:blank");
            AssertJsonValueEquality(actual_json, "$.symbol.value", "ValueString");
        }


        // DvScale : DvOrdered
        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvScale_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvScale() {
                NormalRange = new DvInterval()
                {
                    Lower = new DvOrdinal() {  Value = 10, Symbol = new DvCodedText() {  Value = "SymbolText"} },
                    Upper = new DvOrdinal() { Value = 20, Symbol = new DvCodedText() { Value = "SymbolText" } }
                },
                NormalStatus = new CodePhrase() { CodeString = "CodeString", PreferredTerm = "PreferredTerm", TerminologyId = new TerminologyId() {  VersionId = "1.0.1"} },
                OtherReferenceRanges = [
                    new ReferenceRange() { Meaning = new DvText() { Value = "RefRange1"}, Range = new DvInterval() { Lower = new DvOrdinal() { Value = 50 }, Upper = new DvOrdinal() { Value = 150 }}},
                    new ReferenceRange() { Meaning = new DvText() { Value = "RefRange2"}, Range = new DvInterval() { Lower = new DvOrdinal() { Value = 250 }, Upper = new DvOrdinal() { Value = 350 }}}
                    ],
                Symbol = new DvCodedText()
                {
                    Value = "SymbolText"
                },
                Value = 20.22
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));
            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");

            // assert
            Assert.NotNull(actual_json);
            
            // ._type checks
            AssertJsonValueEquality(actual_json, "$._type", "DV_SCALE");
            AssertJsonValueEquality(actual_json, "$.symbol._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.normal_status.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.normal_range._type", "DV_INTERVAL");
            AssertJsonValueEquality(actual_json, "$.normal_range.lower._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.normal_range.lower.symbol._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.normal_range.upper._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.normal_range.upper.symbol._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].meaning._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range._type", "DV_INTERVAL");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range.lower._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[0].range.upper._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[1].meaning._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[1].range._type", "DV_INTERVAL");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[1].range.lower._type", "DV_ORDINAL");
            AssertJsonValueEquality(actual_json, "$.other_reference_ranges[1].range.upper._type", "DV_ORDINAL");

            // value checks
            AssertJsonValueEquality(actual_json, "$.value", 20.22);
            AssertJsonValueEquality(actual_json, "$.symbol.value", "SymbolText");
            AssertJsonValueEquality(actual_json, "$.normal_status.terminology_id.version_id", "1.0.1");
            AssertJsonValueEquality(actual_json, "$.normal_status.code_string", "CodeString");
            AssertJsonValueEquality(actual_json, "$.normal_status.preferred_term", "PreferredTerm");
            AssertJsonValueEquality(actual_json, "$.normal_range.lower.value", 10);   // fail
            AssertJsonValueEquality(actual_json, "$.normal_range.upper.value", 20);
            AssertJsonValueEquality(actual_json, "$.normal_range.lower.symbol.value", "SymbolText");
            AssertJsonValueEquality(actual_json, "$.normal_range.upper.symbol.value", "SymbolText");            
        }

        
        
        
        // DvQuantity           : DvAmount
        // DvCount              : DvAmount
        // DvProportion         : DvAmount
        // ProportionKind
        // DvAbsoluteQuantity   : DvQuantified

        #endregion


    }
}
