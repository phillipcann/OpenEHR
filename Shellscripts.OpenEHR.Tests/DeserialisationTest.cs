namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Models.PlatformServiceModel;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class DeserialisationTests : BaseTest
    {
        public DeserialisationTests(ITestOutputHelper outputHelper, TestFixture testFixture) 
            : base(outputHelper, testFixture) { }

        [Theory(DisplayName = "Deserialise Composition")]
        [Trait(name: "TestCategory", value: "Unit")]

        [InlineData("Ehr/VersionedEhrStatusResponse.json", typeof(VersionedEhrStatus), "Uid.Value", "5f0841de-9409-4270-919f-6896cc7f4f5e")]
        [InlineData("Ehr/GetEhrResponse.json", typeof(Ehr), "EhrId.Value", "eecf24e0-5ac9-4bfc-b958-475162940444")]
        [InlineData("Ehr/EhrStatusResponse.json", typeof(EhrStatus), "Uid.Value", "c46f12bd-8d0f-49f6-ac36-4f9f4ef18c73::local.ehrbase.org::1")]
        [InlineData("Query/Aql/GetAllEhrIdsResponse.json", typeof(ResultSet), "Query", "SELECT e/ehr_id/value FROM EHR e")]
        [InlineData("Query/Aql/GetCompositionsOfASpecificArchetypeIdResponse.json", typeof(ResultSet), "Query", "SELECT c FROM COMPOSITION c WHERE c/archetype_details/archetype_id/value='openEHR-EHR-COMPOSITION.report-result.v1' LIMIT 10")]
        [InlineData("Ehr/Compositions/1beeaf0a-fbc6-4cef-b1bc-eba1a435fb8e.json", typeof(Composition), "Name.Value", "Minimal Pharmacogenetics diagnostic report")]
        [InlineData("Ehr/Compositions/27df7fcc-0797-42a0-9ad1-ff7428fac33b.json", typeof(Composition), "ArchetypeDetails.ArchetypeId.Value", "openEHR-EHR-COMPOSITION.report.v1")]
        [InlineData("Ehr/Compositions/35b5f439-32d9-49ac-aa93-383b7a7cfc6c.json", typeof(Composition), "Language.TerminologyId.Value", "ISO_639-1")]
        [InlineData("Ehr/Compositions/3e6700c9-9af8-4802-b08c-166d98497cb9.json", typeof(Composition), "ArchetypeNodeId", "openEHR-EHR-COMPOSITION.registereintrag.v1")]
        [InlineData("Ehr/Compositions/657d5d90-3d6a-4969-9a81-b45247b183cb.json", typeof(Composition), "Context.HealthCareFacility.Name", "MHH")]
        [InlineData("Ehr/Compositions/8272bc52-c309-41f0-8cab-1ef820991519.json", typeof(Composition), "Content[0].Data.Events[0].Data.Items[0].Value.Magnitude", 100.0)]
        [InlineData("Ehr/Compositions/841eaeaa-c9fe-475a-b7b2-b60372706b87.json", typeof(Composition), "Content[0].Data.Events[0].Time.Value", "2022-02-03T05:05:06")]
        [InlineData("Ehr/Compositions/90f610c5-0c01-4d79-a75b-61c625b009d5.json", typeof(Composition), "Content[0].Language.CodeString", "de")]
        [InlineData("Ehr/Compositions/95a1abd1-84c2-4a83-8723-e88c06e2fbb2.json", typeof(Composition), "Territory.TerminologyId.Value", "ISO_3166-1")]
        [InlineData("Ehr/Compositions/9f5585c0-920d-43b5-8435-d44cfddce055.json", typeof(Composition), "Content[0].Data.Events[0].Data.Items[1].Name.Value", "Diastolisch")]
        [InlineData("Ehr/Compositions/ae07e3ec-0435-40de-9e9d-34a71b211c20.json", typeof(Composition), "Category.DefiningCode.TerminologyId.Value", "openehr")]
        [InlineData("Ehr/Compositions/c3349156-99a2-47f9-afa8-8d3d1c25497d.json", typeof(Composition), "Composer.Name", "Max Mustermann")]
        [InlineData("Ehr/Compositions/cb8b682b-c2e6-4265-8405-a246ec0c86a1.json", typeof(Composition), "Content[0].Data.Events[0].Data.Items[1].Items[1].Value.Symbol.Value", "Kein selbstst�ndiges Gehen m�glich")]
        [InlineData("Ehr/Compositions/cdc46572-9074-451e-aeee-843ae2e44ecd.json", typeof(Composition), "Context.StartTime.Value", "2024-09-08T18:29:13.141804578Z")]
        [InlineData("Ehr/Compositions/d226a782-65d1-40c3-9ed4-87de2a81b15a.json", typeof(Composition), "Content[0].Items[0].WorkflowId.Id.Value", "9eb02724-847a-3b03-98c6-de27f3a95b69")]
        [InlineData("Ehr/Compositions/fee1f585-60ee-40c7-b07f-017e2f9318c2.json", typeof(Composition), "Uid.Value", "fee1f585-60ee-40c7-b07f-017e2f9318c2::local.ehrbase.org::1")]
        [InlineData("Ehr/VersionedCompositions/95a1abd1-84c2-4a83-8723-e88c06e2fbb2.json", typeof(VersionedComposition), "OwnerId.Id.Value", "b8cee9a8-d84e-4ed7-b40d-8c48215840b2")]
        public async Task Can_DeserialiseCompositionResponse_Success(string assetFile, Type returnType, string fieldToCheck, object expectedValue)
        {
            // arrange            
            var options = Services?.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync(assetFile);

            OutputHelper?.WriteLine($"Loaded content from Asset File: {assetFile}. ContentLength: {assetFileContent.Length}");


            // act
            var dataObject = JsonSerializer.Deserialize(assetFileContent, returnType, options);            

            
            // assert
            Assert.NotNull(dataObject);
            var actualValue = dataObject.GetNestedPropertyValue(fieldToCheck);
            
            OutputHelper?.WriteLine($"Actual Value: {actualValue}. Expected Value: {expectedValue}");
            OutputHelper?.WriteLine($"Actual Value Type: {actualValue?.GetType().Name}. Expected Value Type: {expectedValue.GetType().Name}");

            Assert.NotNull(actualValue);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}