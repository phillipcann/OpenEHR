namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Shellscripts.OpenEHR.Extensions;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class DeserialisationTests : BaseTest
    {
        public DeserialisationTests(ITestOutputHelper outputHelper, TestFixture testFixture) 
            : base(outputHelper, testFixture) { }

        [Theory()]
        [Trait(name: "Category", value: "Unit Test")]
        [InlineData("Ehr/VersionedEhrStatusResponse.json", typeof(VersionedEhrStatus), "Uid.Value", "5f0841de-9409-4270-919f-6896cc7f4f5e")]
        [InlineData("Ehr/GetEhrResponse.json", typeof(Ehr), "EhrId.Value", "eecf24e0-5ac9-4bfc-b958-475162940444")]
        public async Task Can_Deserialise_SpecificResponse_ToSpecificType_Success(string assetFile, Type returnType, string fieldToCheck, object expectedValue)
        {
            // arrange
            var options = Services.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync(assetFile);

            // act
            var dataObject = JsonSerializer.Deserialize(assetFileContent, returnType, options);

            // assert
            var actualValue = dataObject.GetNestedPropertyValue(fieldToCheck);

            Assert.NotNull(dataObject);
            Assert.NotNull(actualValue);
            Assert.Equal(expectedValue, actualValue);
        }


        [Theory(DisplayName = "Deserialise Composition")]
        [Trait(name: "Category", value: "Unit Test")]
        [InlineData("cdc46572-9074-451e-aeee-843ae2e44ecd")]
        [InlineData("35b5f439-32d9-49ac-aa93-383b7a7cfc6c")]
        [InlineData("27df7fcc-0797-42a0-9ad1-ff7428fac33b")]
        [InlineData("ae07e3ec-0435-40de-9e9d-34a71b211c20")]
        [InlineData("d226a782-65d1-40c3-9ed4-87de2a81b15a")]
        [InlineData("fee1f585-60ee-40c7-b07f-017e2f9318c2")]
        [InlineData("95a1abd1-84c2-4a83-8723-e88c06e2fbb2")]
        [InlineData("1beeaf0a-fbc6-4cef-b1bc-eba1a435fb8e")]
        [InlineData("c3349156-99a2-47f9-afa8-8d3d1c25497d")]
        public async Task Can_DeserialiseCompositionResponse_Success(string assetFile)
        {
            // arrange
            
            var options = Services.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync($"Ehr/Compositions/{assetFile}.json");

            // act
            var dataObject = JsonSerializer.Deserialize<Composition>(assetFileContent, options);            
            
            // TODO : Got as far as Content. Needs a new Converter for Observation

            // assert
            Assert.NotNull(dataObject);

            // at least check the uid has been deserialised from the json. this also checks the "Root" extension
            // method works to extract just the guid part of the identifier from the Uid
            Assert.Equal(dataObject.Uid.Root().Value, assetFile);
        }


        [Theory(DisplayName = "Deserialise VersionedComposition")]
        [Trait(name: "Category", value: "Unit Test")]
        [InlineData("95a1abd1-84c2-4a83-8723-e88c06e2fbb2")]

        public async Task Can_DeserialiseVersionedCompositionResponse_Success(string assetFile)
        {
            var options = Services.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync($"Ehr/VersionedCompositions/{assetFile}.json");

            // act
            var dataObject = JsonSerializer.Deserialize<VersionedComposition>(assetFileContent, options);

            // assert
            Assert.NotNull(dataObject);
        }
    }
}
