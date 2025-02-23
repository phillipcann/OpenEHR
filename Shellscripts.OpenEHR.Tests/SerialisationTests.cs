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
                    IsModifiable = false,
                    IsQueryable = false
                },
                TimeCreated = new DvDateTime() { Value = "2024-08-30T21:45:33.389606Z" }
            };

            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_ehr, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "EHR");
            AssertJsonValueEquality(actual_json, "$.system_id._type", "HIER_OBJECT_ID");
            AssertJsonValueEquality(actual_json, "$.system_id.value", "local.ehrbase.org");
            AssertJsonValueEquality(actual_json, "$.ehr_id._type", "HIER_OBJECT_ID");
            AssertJsonValueEquality(actual_json, "$.ehr_id.value", "eecf24e0-5ac9-4bfc-b958-475162940444");
            AssertJsonValueEquality(actual_json, "$.time_created._type", "DV_DATE_TIME");
            AssertJsonValueEquality(actual_json, "$.time_created.value", "2024-08-30T21:45:33.389606Z");
            AssertJsonValueEquality(actual_json, "$.ehr_status._type", "EHR_STATUS");
            AssertJsonValueEquality(actual_json, "$.ehr_status.uid._type", "OBJECT_VERSION_ID");
            AssertJsonValueEquality(actual_json, "$.ehr_status.uid.value", "5f0841de-9409-4270-919f-6896cc7f4f5e::local.ehrbase.org::1");
            AssertJsonValueEquality(actual_json, "$.ehr_status.archetype_node_id", "openEHR-EHR-EHR_STATUS.generic.v1");
            AssertJsonValueEquality(actual_json, "$.ehr_status.name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.ehr_status.name.value", "EHR Status");
            AssertJsonValueEquality(actual_json, "$.ehr_status.is_queryable", false);
            AssertJsonValueEquality(actual_json, "$.ehr_status.is_modifiable", false);
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
                Content =
                [
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
                            Items =
                            [
                                new Element()
                                {
                                    Name = new DvText() { Value = "Measurement Device"},
                                    ArchetypeNodeId = "at0025",
                                    Value = new DvText()
                                    {
                                        Value = "Automatic Sphygmomanometer"
                                    }
                                }
                            ]
                        }
                    }
                ],
                Context = new EventContext()
                {
                    StartTime = new DvDateTime() { Value = "2024-01-01T10:00:00.000Z" },
                    EndTime = new DvDateTime() { Value = "2024-01-01T12:00:00.000Z" },
                    Location = "Location",
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

            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_composition, serialiserOptions));

            // assert
            
            Assert.NotNull(actual_json);

            AssertJsonValueEquality(actual_json, "$._type", "COMPOSITION");
            AssertJsonValueEquality(actual_json, "$.language.code_string", "en");
            AssertJsonValueEquality(actual_json, "$.language.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.language.terminology_id.value", "ISO_639-1");
            AssertJsonValueEquality(actual_json, "$.territory.code_string", "US");
            AssertJsonValueEquality(actual_json, "$.territory.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.territory.terminology_id.value", "ISO_3166-1");
            AssertJsonValueEquality(actual_json, "$.context.start_time._type", "DV_DATE_TIME");
            AssertJsonValueEquality(actual_json, "$.context.start_time.value", "2024-01-01T10:00:00.000Z");
            AssertJsonValueEquality(actual_json, "$.context.end_time.value", "2024-01-01T12:00:00.000Z");
            AssertJsonValueEquality(actual_json, "$.context.location", "Location");
            AssertJsonValueEquality(actual_json, "$.context.setting.value", "General Practice");
            AssertJsonValueEquality(actual_json, "$.context.setting.defining_code.code_string", "238");
            AssertJsonValueEquality(actual_json, "$.context.setting.defining_code.terminology_id._type", "TERMINOLOGY_ID");
            AssertJsonValueEquality(actual_json, "$.context.setting.defining_code.terminology_id.value", "openehr");
            AssertJsonValueEquality(actual_json, "$.content[0].name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.content[0].name.value", "Blood Pressure");
            AssertJsonValueEquality(actual_json, "$.content[0].archetype_details.archetype_id._type", "ARCHETYPE_ID");
            AssertJsonValueEquality(actual_json, "$.content[0].archetype_details.archetype_id.value", "openEHR-EHR-OBSERVATION.blood_pressure.v1");
            AssertJsonValueEquality(actual_json, "$.content[0].archetype_details.rm_version", "1.0.4");
            AssertJsonValueEquality(actual_json, "$.content[0].data.name.value", "Event Series");
            AssertJsonValueEquality(actual_json, "$.content[0].data.archetype_node_id", "at0001");
            AssertJsonValueEquality(actual_json, "$.content[0].data.origin.value", "2024-01-01T10:00:00Z");
            AssertJsonValueEquality(actual_json, "$.content[0].data.events[0].name.value", "Any Event");
            AssertJsonValueEquality(actual_json, "$.content[0].data.events[0].archetype_node_id", "at0006");
            AssertJsonValueEquality(actual_json, "$.content[0].data.events[0].time.value", "2024-01-01T10:00:00Z");
            AssertJsonValueEquality(actual_json, "$.content[0].data.events[0].data._type", "ITEM_TREE");


            AssertJsonValueEquality(actual_json, "$.content[0].protocol.name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.content[0].protocol.name.value", "Protocol");
            AssertJsonValueEquality(actual_json, "$.content[0].protocol.archetype_node_id", "at0011");
            AssertJsonValueEquality(actual_json, "$.content[0].protocol.items[0].name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.content[0].protocol.items[0].name.value", "Measurement Device");
            AssertJsonValueEquality(actual_json, "$.content[0].protocol.items[0].archetype_node_id", "at0025");
            AssertJsonValueEquality(actual_json, "$.content[0].protocol.items[0].value.value", "Automatic Sphygmomanometer");
            AssertJsonValueEquality(actual_json, "$.name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.name.value", "Blood Pressure Measurement");
            AssertJsonValueEquality(actual_json, "$.archetype_details.rm_version", "1.0.4");
            AssertJsonValueEquality(actual_json, "$.archetype_details.archetype_id._type", "ARCHETYPE_ID");
            AssertJsonValueEquality(actual_json, "$.archetype_details.archetype_id.value", "openEHR-EHR-COMPOSITION.observation.v1");
            AssertJsonValueEquality(actual_json, "$.archetype_details.template_id._type", "TEMPLATE_ID");
            AssertJsonValueEquality(actual_json, "$.archetype_details.template_id.value", "Blood Pressure Template");
        }


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_Serialise_ContentItemArray_Success()
        {
            await Task.Run(() => { });

            // arrange
            string expected_json = await LoadAssetAsync("Serialisation/ContentItemArrayWithThreeDifferentParts.json");
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            var mock_contentItemArray = new ContentItem[]
            {
                new AdminEntry()
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
                    },

                    // These are the AdminEntry Bits
                    Data = new ItemSingle() { Item = new Element() { Name = new DvText() { Value = "Element" } } }
                },
                new Section()
                {
                    ArchetypeDetails = new Archetyped() { ArchetypeId = new ArchetypeId() { Value = "02-archetype-id" } },
                    ArchetypeNodeId = "02-archetype-node-id",
                    Name = new DvText() { Value = "02-name" },
                    Uid = new UidBasedId() { Value = "02-uid" },
                    Links = new Link[]
                    {
                        new Link() {
                            Meaning = new DvText() { Value = "02-link-meaning "},
                            Target = new DvEhrUri() { Value = "ds://02-link-target"},
                            Type = new DvText() { Value = "02-link-type"}
                        }
                    },

                    // These are the Section bits
                    Items = new ContentItem[] 
                    { 
                        new Evaluation() { Protocol = null, GuidelineId = null },
                        new Instruction() { Narrative = new DvText() { Value = "Narrative"}, ExpiryTime = new DvDateTime() { Value = "2025-01-01T00:00:00.000Z"} }
                    },

                },
                new Observation()
                {
                    ArchetypeDetails = new Archetyped() { ArchetypeId = new ArchetypeId() { Value = "03-archetype-id" } },
                    ArchetypeNodeId = "03-archetype-node-id",
                    Name = new DvText() { Value = "03-name" },
                    Uid = new UidBasedId() { Value = "03-uid" },
                    Subject = new PartySelf() { ExternalReference = new PartyRef() { Namespace = "03-subject-extref", Type = "03-subject-type"}},
                    Links = new Link[]
                    {
                        new Link() {
                            Meaning = new DvText() { Value = "03-link-meaning "},
                            Target = new DvEhrUri() { Value = "ds://03-link-target"},
                            Type = new DvText() { Value = "03-link-type"}
                        }
                    }
                }
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_contentItemArray, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            Assert.True(await AreJsonStringsEqualAsync(actual_json, expected_json));
        }


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_Serialise_EhrStatus_Success()
        {
            // arrange
            EhrStatus mock_ehr_status = new EhrStatus()
            {
                ArchetypeNodeId = "openEHR-EHR-EHR_STATUS.generic.v1",
                Subject = new PartySelf()
                {
                    ExternalReference = new PartyRef()
                    {
                        Namespace = "https://fhir_nhs_uk/nhsnumber",
                        Type = "PERSON",
                        Id = new HierObjectId()
                        {
                            Value = "8888812345"
                        }
                    }
                },
                IsModifiable = true,
                IsQueryable = true
            };

            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_ehr_status, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$._type", "EHR_STATUS");
            AssertJsonValueEquality(actual_json, "$.archetype_node_id", "openEHR-EHR-EHR_STATUS.generic.v1");
            AssertJsonValueEquality(actual_json, "$.subject._type", "PARTY_SELF");
            AssertJsonValueEquality(actual_json, "$.subject.external_ref._type", "PARTY_REF");
            AssertJsonValueEquality(actual_json, "$.subject.external_ref.namespace", "https://fhir_nhs_uk/nhsnumber");
            AssertJsonValueEquality(actual_json, "$.subject.external_ref.type", "PERSON");
            AssertJsonValueEquality(actual_json, "$.subject.external_ref.id._type", "HIER_OBJECT_ID");
            AssertJsonValueEquality(actual_json, "$.subject.external_ref.id.value", "8888812345");
            AssertJsonValueEquality(actual_json, "$.is_queryable", true);
            AssertJsonValueEquality(actual_json, "$.is_modifiable", true);
        }


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_Serialise_ItemArraySegment_Success()
        {
            // arrange
            var serialiserOptions = Services?.GetRequiredService<JsonSerializerOptions>();

            History<ItemStructure> mock_data = new History<ItemStructure>
            {
                Events = [
                    new PointEvent<ItemStructure>
                    {                        
                        Data = new ItemTree()
                        {
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
                                new Cluster()
                                {
                                    Name = new DvText() { Value = "Distolic" },
                                    Items = [
                                        new Element() { Name = new DvText() { Value = "SomeElement" } },
                                        new Cluster() { Name = new DvText() { Value = "SomeCluster" } }
                                    ]
                                }
                            ]
                        }
                    }
                ]
            };

            // act
            var actual_json = await Task.Run(() => JsonSerializer.Serialize(mock_data, serialiserOptions));

            // assert
            Assert.NotNull(actual_json);
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[0].name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[0].name.value", "Systolic");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[0].archetype_node_id", "at0004");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[0].value._type", "DV_QUANTITY");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[0].value.magnitude", 120);
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[0].value.units", "mmHg");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[1].name.value", "Distolic");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[1].items[0].name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[1].items[0].name.value", "SomeElement");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[1].items[1].name._type", "DV_TEXT");
            AssertJsonValueEquality(actual_json, "$.events[0].data.items[1].items[1].name.value", "SomeCluster");
        }






    }
}