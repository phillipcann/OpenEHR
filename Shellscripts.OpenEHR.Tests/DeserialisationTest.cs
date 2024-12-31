namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class DeserialisationTests : BaseTest
    {
        public DeserialisationTests(ITestOutputHelper outputHelper, TestFixture testFixture) 
            : base(outputHelper, testFixture) { }

        [Theory(DisplayName = "Deserialise Ehr Responses")]
        [Trait(name: "Category", value: "Unit Test")]
        [InlineData("Ehr/VersionedEhrStatusResponse.json", typeof(VersionedEhrStatus), "Uid.Value", "5f0841de-9409-4270-919f-6896cc7f4f5e")]
        [InlineData("Ehr/GetEhrResponse.json", typeof(Ehr), "EhrId.Value", "eecf24e0-5ac9-4bfc-b958-475162940444")]
        [InlineData("Ehr/EhrStatusResponse.json", typeof(EhrStatus), "Uid.Value", "c46f12bd-8d0f-49f6-ac36-4f9f4ef18c73::local.ehrbase.org::1")]
        public async Task Can_Deserialise_SpecificResponse_ToSpecificType_Success(string assetFile, Type returnType, string fieldToCheck, object expectedValue)
        {
            // arrange
            var options = Services?.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync(assetFile);

            // act
            var dataObject = JsonSerializer.Deserialize(assetFileContent, returnType, options);

            // assert
            Assert.NotNull(dataObject);
            var actualValue = dataObject.GetNestedPropertyValue(fieldToCheck);
            
            Assert.NotNull(actualValue);
            Assert.Equal(expectedValue, actualValue);
        }


        [Theory(DisplayName = "Deserialise Composition")]
        [Trait(name: "Category", value: "Unit Test")]
        [InlineData("1beeaf0a-fbc6-4cef-b1bc-eba1a435fb8e", "Name.Value", "Minimal Pharmacogenetics diagnostic report")]
        [InlineData("27df7fcc-0797-42a0-9ad1-ff7428fac33b", "ArchetypeDetails.ArchetypeId.Value", "openEHR-EHR-COMPOSITION.report.v1")]
        [InlineData("35b5f439-32d9-49ac-aa93-383b7a7cfc6c", "Language.TerminologyId.Value", "ISO_639-1")]
        [InlineData("3e6700c9-9af8-4802-b08c-166d98497cb9", "ArchetypeNodeId", "openEHR-EHR-COMPOSITION.registereintrag.v1")]
        [InlineData("657d5d90-3d6a-4969-9a81-b45247b183cb", "Context.HealthCareFacility.Name", "MHH")]
        [InlineData("8272bc52-c309-41f0-8cab-1ef820991519", "Content[0].Data.Events[0].Data.Items[0].Value.Magnitude", 100.0)]
        [InlineData("841eaeaa-c9fe-475a-b7b2-b60372706b87", "Content[0].Data.Events[0].Time.Value", "2022-02-03T05:05:06")]
        [InlineData("90f610c5-0c01-4d79-a75b-61c625b009d5", "Content[0].Language.CodeString", "de")]
        [InlineData("95a1abd1-84c2-4a83-8723-e88c06e2fbb2", "Territory.TerminologyId.Value", "ISO_3166-1")]
        [InlineData("9f5585c0-920d-43b5-8435-d44cfddce055", "Content[0].Data.Events[0].Data.Items[1].Name.Value", "Diastolisch")]
        [InlineData("ae07e3ec-0435-40de-9e9d-34a71b211c20", "Category.DefiningCode.TerminologyId.Value", "openehr")]
        [InlineData("c3349156-99a2-47f9-afa8-8d3d1c25497d", "Composer.Name", "Max Mustermann")]
        [InlineData("cb8b682b-c2e6-4265-8405-a246ec0c86a1", "Content[0].Data.Events[0].Data.Items[1].Items[1].Value.Symbol.Value", "Kein selbstst�ndiges Gehen m�glich")]
        [InlineData("cdc46572-9074-451e-aeee-843ae2e44ecd", "Context.StartTime.Value", "2024-09-08T18:29:13.141804578Z")]
        [InlineData("d226a782-65d1-40c3-9ed4-87de2a81b15a", "Content[0].Items[0].WorkflowId.Id.Value", "9eb02724-847a-3b03-98c6-de27f3a95b69")]
        [InlineData("fee1f585-60ee-40c7-b07f-017e2f9318c2", "Uid.Value", "fee1f585-60ee-40c7-b07f-017e2f9318c2::local.ehrbase.org::1")]
        public async Task Can_DeserialiseCompositionResponse_Success(string assetFile, string fieldToCheck, object expectedValue)
        {
            // arrange            
            var options = Services?.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync($"Ehr/Compositions/{assetFile}.json");

            // act
            var dataObject = JsonSerializer.Deserialize<Composition>(assetFileContent, options);            
            
            // assert
            Assert.NotNull(dataObject);
            var actualValue = dataObject.GetNestedPropertyValue(fieldToCheck);

            Assert.NotNull(actualValue);
            Assert.Equal(expectedValue, actualValue);
        }


        [Theory(DisplayName = "Deserialise VersionedComposition")]
        [Trait(name: "Category", value: "Unit Test")]
        [InlineData("95a1abd1-84c2-4a83-8723-e88c06e2fbb2", "OwnerId.Id.Value", "b8cee9a8-d84e-4ed7-b40d-8c48215840b2")]

        public async Task Can_DeserialiseVersionedCompositionResponse_Success(string assetFile, string fieldToCheck, string expectedValue)
        {
            var options = Services?.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync($"Ehr/VersionedCompositions/{assetFile}.json");

            // act // assert
            var dataObject = JsonSerializer.Deserialize<VersionedComposition>(assetFileContent, options);
            Assert.NotNull(dataObject);

            var actualValue = dataObject.GetNestedPropertyValue(fieldToCheck);
            Assert.NotNull(actualValue);
            Assert.Equal(expectedValue, actualValue);

        }
    }
}
