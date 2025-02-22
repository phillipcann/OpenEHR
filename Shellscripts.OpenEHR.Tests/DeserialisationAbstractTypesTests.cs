namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class DeserialisationAbstractTypesTests : BaseTest
    {

        public DeserialisationAbstractTypesTests(ITestOutputHelper outputHelper, TestFixture testFixture)
            : base(outputHelper, testFixture) { }


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseEventItemTree_Success()
        {
            // arrange
            var options = Services?.GetRequiredService<JsonSerializerOptions>();

            string test_json_segment = @"[{
            ""_type"": ""OBSERVATION"",            
            ""data"": {
                ""name"": {
                    ""_type"": ""DV_TEXT"",
                    ""value"": ""Historie""
                },
                ""origin"": {
                    ""_type"": ""DV_DATE_TIME"",
                    ""value"": ""2022-02-03T08:05:06""
                },
                ""events"": [
                    {
                        ""_type"": ""POINT_EVENT"",
                        ""name"": {
                            ""_type"": ""DV_TEXT"",
                            ""value"": ""Beliebiges Ereignis""
                        },
                        ""time"": {
                            ""_type"": ""DV_DATE_TIME"",
                            ""value"": ""2022-02-03T08:05:06""
                        },
                        ""data"": {
                            ""_type"": ""ITEM_TREE"",
                            ""name"": {
                                ""_type"": ""DV_TEXT"",
                                ""value"": ""Blutdruck""
                            },
                            ""items"": [
                                {
                                    ""_type"": ""ELEMENT"",
                                    ""name"": {
                                        ""_type"": ""DV_TEXT"",
                                        ""value"": ""Systolisch""
                                    },
                                    ""value"": {
                                        ""_type"": ""DV_QUANTITY"",
                                        ""units"": ""mm[Hg]"",
                                        ""magnitude"": 89.0
                                    },
                                    ""archetype_node_id"": ""at0004""
                                },
                                {
                                    ""_type"": ""ELEMENT"",
                                    ""name"": {
                                        ""_type"": ""DV_TEXT"",
                                        ""value"": ""Diastolisch""
                                    },
                                    ""value"": {
                                        ""_type"": ""DV_QUANTITY"",
                                        ""units"": ""mm[Hg]"",
                                        ""magnitude"": 145.0
                                    },
                                    ""archetype_node_id"": ""at0005""
                                }
                            ],
                            ""archetype_node_id"": ""at0003""
                        },
                        ""archetype_node_id"": ""at0006""
                    }
                ]}}]";

            var test_json_document = JsonDocument.Parse(test_json_segment);

            // act
            var dataObject = await Task.Run(() => JsonSerializer.Deserialize<ContentItem[]>(test_json_document, options));


            // assert
            Assert.NotNull(dataObject);
            Assert.True(dataObject.Any());
            var obs = dataObject[0];

            Assert.Equal("Historie", obs.GetNestedPropertyValue("Data.Name.Value"));
            Assert.Equal("2022-02-03T08:05:06", obs.GetNestedPropertyValue("Data.Origin.Value"));            
            Assert.Equal("Beliebiges Ereignis", obs.GetNestedPropertyValue("Data.Events[0].Name.Value"));
            Assert.Equal("2022-02-03T08:05:06", obs.GetNestedPropertyValue("Data.Events[0].Time.Value"));
            Assert.Equal("Blutdruck", obs.GetNestedPropertyValue("Data.Events[0].Data.Name.Value"));
            Assert.Equal("Systolisch", obs.GetNestedPropertyValue("Data.Events[0].Data.Items[0].Name.Value"));
            Assert.Equal("mm[Hg]", obs.GetNestedPropertyValue("Data.Events[0].Data.Items[0].Value.Units"));
            Assert.Equal(89.0, obs.GetNestedPropertyValue("Data.Events[0].Data.Items[0].Value.Magnitude"));
            Assert.Equal("at0004", obs.GetNestedPropertyValue("Data.Events[0].Data.Items[0].ArchetypeNodeId"));
            Assert.Equal("Diastolisch", obs.GetNestedPropertyValue("Data.Events[0].Data.Items[1].Name.Value"));
            Assert.Equal("mm[Hg]", obs.GetNestedPropertyValue("Data.Events[0].Data.Items[1].Value.Units"));
            Assert.Equal(145.0, obs.GetNestedPropertyValue("Data.Events[0].Data.Items[1].Value.Magnitude"));
            Assert.Equal("at0005", obs.GetNestedPropertyValue("Data.Events[0].Data.Items[1].ArchetypeNodeId"));
            Assert.Equal("at0003", obs.GetNestedPropertyValue("Data.Events[0].Data.ArchetypeNodeId"));
            Assert.Equal("at0006", obs.GetNestedPropertyValue("Data.Events[0].ArchetypeNodeId"));
        }

    }
}
