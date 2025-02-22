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


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseDvBoolean_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();
            var data = new DvBoolean() { Value = true };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_BOOLEAN");
            AssertJsonValueEquality(actual_json, "$.value", true);

            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");
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

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_STATE");
            AssertJsonValueEquality(actual_json, "$.value._type", "DV_CODED_TEXT");
            AssertJsonValueEquality(actual_json, "$.value.value", "TextField");
            AssertJsonValueEquality(actual_json, "$.value.formatting", "FormattingText");

            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");
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

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_IDENTIFIER");
            AssertJsonValueEquality(actual_json, "$.assigner", "Assigner");
            AssertJsonValueEquality(actual_json, "$.id", "Id");
            AssertJsonValueEquality(actual_json, "$.issuer", "Issuer");
            AssertJsonValueEquality(actual_json, "$.type", "Type");

            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");
        }

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
                            Name = "",
                            Value = "",
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
                        VersionId = "T_Value2",
                        Value = "2.0.1"
                    }
                }
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(data, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.value", "Some Text Value");
            AssertJsonValueEquality(actual_json, "$.hyperlink.value", "https://about:blank");
            AssertJsonValueEquality(actual_json, "$.formatting", "Formatting");

            // TODO : more paths

            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");
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

            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");
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

            OutputHelper?.WriteLine($"Actual Json: \n{actual_json}");
        }





    }
}
