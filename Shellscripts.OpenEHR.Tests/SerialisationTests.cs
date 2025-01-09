namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;

    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Tests.Context;

    using Xunit;
    using Xunit.Abstractions;


    public class SerialisationTests : BaseTest
    {

        public SerialisationTests(ITestOutputHelper outputHelper, TestFixture testFixture)
            : base(outputHelper, testFixture) { }


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_Serialise_Ehr_Success()
        {
            // arrange
            Ehr mock_ehr = new Ehr()
            {
                SystemId = new HierObjectId() { Value = "local.ehrbase.org" },
                EhrId = new HierObjectId() { Value = "eecf24e0-5ac9-4bfc-b958-475162940444" },
                EhrStatus = new EhrStatus()
                {
                    Uid = new ObjectVersionId() { Value = "5f0841de-9409-4270-919f-6896cc7f4f5e::local.ehrbase.org::1" },
                    ArchetypeNodeId = "openEHR-EHR-EHR_STATUS.generic.v1",
                    Name = new DvText() { Value = "EHR Status" },
                    IsModifiable = true,
                    IsQueryable = true
                },
                TimeCreated = new DvDateTime() { Value = "2024-08-30T21:45:33.389606Z" }
            };

            string expected_json = await LoadAssetAsync("Serialisation/Ehr.json");
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_ehr, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            Assert.True(await AreJsonStringsEqualAsync(actual_json, expected_json));
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_Serialise_Composition_WithObservation_Success()
        {
            // arrange
            Composition mock_composition = new Composition()
            {
                Name = new DvText() { Value = "Blood Pressure Measurement" },
                ArchetypeDetails = new Archetyped()
                {
                    ArchetypeId = new ArchetypeId() { Value = "openEHR-EHR-COMPOSITION.observation.v1" },
                    TemplateId = new TemplateId() { Value = "Blood Pressure Template" },
                    RmVersion = "1.0.4"
                },
                Content = new ContentItem[]
                {
                    new Observation()
                    {
                        Name = new DvText() { Value = "Blood Pressure" },
                        ArchetypeDetails = new Archetyped()
                        {
                            ArchetypeId = new ArchetypeId() { Value = "openEHR-EHR-OBSERVATION.blood_pressure.v1" },
                            RmVersion = "1.0.4"
                        },
                        Data = new History<ItemStructure>
                        {
                            Name = new DvText() { Value = "Event Series" },
                            ArchetypeNodeId = "at0001",
                            Origin = new DvDateTime()
                            {
                                Value = "2024-01-01T10:00:00Z"
                            },
                            Events = [
                                new PointEvent<ItemStructure>
                                {
                                    Name = new DvText() { Value = "Any Event" },
                                    ArchetypeNodeId = "at0006",
                                    Time = new DvDateTime() { Value = "2024-01-01T10:00:00Z" },
                                    Data = new ItemTree()
                                    {
                                        Name = new DvText() { Value = "Blood Pressure Data" },
                                        ArchetypeNodeId = "at0003",
                                        Items = [
                                            new Element()
                                            {
                                                Name = new DvText() { Value = "Systolic" },
                                                ArchetypeNodeId = "at0004",
                                                Value = new DvQuantity()
                                                {
                                                    Magnitude = 120,
                                                    Units = "mmHg"
                                                }
                                            },
                                            new Element()
                                            {
                                                Name = new DvText() { Value = "Diastolic" },
                                                ArchetypeNodeId = "at0005",
                                                Value = new DvQuantity()
                                                {
                                                    Magnitude = 80,
                                                    Units = "mmHg"
                                                }
                                            },
                                            new Element()
                                            {
                                                Name = new DvText() { Value = "Mean Arterial Pressure" },
                                                ArchetypeNodeId = "at0057",
                                                Value = new DvQuantity()
                                                {
                                                    Magnitude = 93.3,
                                                    Units = "mmHg"
                                                }
                                            }
                                        ]
                                    }
                                }
                            ]
                        },
                        Protocol = new ItemTree()
                        {
                            Name = new DvText() { Value = "Protocol" },
                            ArchetypeNodeId = "at0011",
                            Items = new Item[]
                            {
                                new Element()
                                {
                                    Name = new DvText() { Value = "Measurement Device"},
                                    ArchetypeNodeId = "at0025",
                                    Value = new DvText()
                                    {
                                        Value = "Automatic Sphygmomanometer"
                                    }
                                }
                            }
                        }
                    }
                },
                Context = new EventContext() {
                    StartTime = new DvDateTime() { Value = "2024-01-01T10:00:00Z" },
                    Setting = new DvCodedText()
                    {
                        Value = "General Practice",
                        DefiningCode = new CodePhrase()
                        {
                            TerminologyId = new TerminologyId() { Value = "openehr" },
                            CodeString = "238"
                        }
                    }
                },
                Language = new CodePhrase()
                {
                    TerminologyId = new TerminologyId() { Value = "ISO_639-1" },
                    CodeString = "en"
                },
                Territory = new CodePhrase()
                {
                    TerminologyId = new TerminologyId() { Value = "ISO_3166-1" },
                    CodeString = "US"
                }
            };

            string expected_json = await LoadAssetAsync("Serialisation/CompositionWithObservation.json");
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_composition, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            Assert.True(await AreJsonStringsEqualAsync(actual_json, expected_json));
        }


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_Serialise_ContentItemArray_Success()
        {
            await Task.Run(() => { });

            // arrange
            string expected_json = await LoadAssetAsync("Serialisation/ContentItemArrayWithThreeDifferentParts.json");
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            var mock_contentItemArray = new List<ContentItem>
            {
                new ContentItem()
                {
                    ArchetypeDetails = new Archetyped() { ArchetypeId = new ArchetypeId() { Value = "01-archetype-id" } },
                    ArchetypeNodeId = "01-archetype-node-id",
                    Name = new DvText() { Value = "01-name" },
                    Uid = new UidBasedId() { Value = "01-uid" },
                    Links = new Link[]
                    {
                        new Link() { 
                            Meaning = new DvText() { Value = "01-link-meaning "}, 
                            Target = new DvEhrUri() { Value = "ds://01-link-target"},
                            Type = new DvText() { Value = "01-link-type"}
                        }
                    }
                },
                new Section()
                {
                    ArchetypeDetails = new Archetyped() { ArchetypeId = new ArchetypeId() { Value = "02-archetype-id" } },
                    ArchetypeNodeId = "02-archetype-node-id",
                    Name = new DvText() { Value = "02-name" },
                    Uid = new UidBasedId() { Value = "02-uid" },
                    Items = new ContentItem[] { },
                    Links = new Link[]
                    {
                        new Link() {
                            Meaning = new DvText() { Value = "02-link-meaning "},
                            Target = new DvEhrUri() { Value = "ds://02-link-target"},
                            Type = new DvText() { Value = "02-link-type"}
                        }
                    }

                },
                new Observation()
                {
                    ArchetypeDetails = new Archetyped() { ArchetypeId = new ArchetypeId() { Value = "03-archetype-id" } },
                    ArchetypeNodeId = "03-archetype-node-id",
                    Name = new DvText() { Value = "03-name" },
                    Uid = new UidBasedId() { Value = "03-uid" },
                    Subject = new PartyProxy() { ExternalReference = new PartyRef() { Namespace = "03-subject-extref", Type = "03-subject-type"}},
                    Links = new Link[]
                    {
                        new Link() {
                            Meaning = new DvText() { Value = "03-link-meaning "},
                            Target = new DvEhrUri() { Value = "ds://03-link-target"},
                            Type = new DvText() { Value = "03-link-type"}
                        }
                    }
                }
            }.ToArray();

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_contentItemArray, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            Assert.True(await AreJsonStringsEqualAsync(actual_json, expected_json));
        }
    }
}
