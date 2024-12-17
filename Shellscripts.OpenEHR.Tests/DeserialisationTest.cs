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

        [Fact]
        [Trait(name: "Category", value: "Unit Test")]
        public async Task Can_DeserialiseVersionedEhrStatusResponse_Success()
        {
            // arrange
            var options = Services.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync("Ehr/VersionedEhrStatusResponse.json");

            // act
            var dataObject = JsonSerializer.Deserialize<VersionedEhrStatus>(assetFileContent, options);
            var serialisedObject = JsonSerializer.Serialize(dataObject, options);

            // assert
            Assert.NotNull(dataObject);
            Assert.NotNull(serialisedObject);
            Assert.NotEmpty(serialisedObject);
        }

        [Fact]
        [Trait(name: "Category", value: "Unit Test")]
        public async Task Can_DeserialiserEhrResponse_Success()
        {
            // arrange
            var options = Services.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync("Ehr/GetEhrResponse.json");

            // act
            var dataObject = JsonSerializer.Deserialize<Ehr>(assetFileContent, options);
            var serialisedObject = JsonSerializer.Serialize(dataObject, options);
            
            // TODO : Whilst this works, this MIGHT be problematic with the serialise piece including null properties in segments that shouldnt have the properties at all.

            // assert
            Assert.NotNull(dataObject);
            Assert.NotNull(serialisedObject);
            Assert.NotEmpty(serialisedObject);

        }

        [Theory]
        [Trait(name: "Category", value: "Unit Test")]
        [InlineData("cdc46572-9074-451e-aeee-843ae2e44ecd")]
        [InlineData("35b5f439-32d9-49ac-aa93-383b7a7cfc6c")]
        [InlineData("27df7fcc-0797-42a0-9ad1-ff7428fac33b")]
        [InlineData("ae07e3ec-0435-40de-9e9d-34a71b211c20")]
        [InlineData("d226a782-65d1-40c3-9ed4-87de2a81b15a")]
        [InlineData("fee1f585-60ee-40c7-b07f-017e2f9318c2")]
        [InlineData("95a1abd1-84c2-4a83-8723-e88c06e2fbb2")]
        public async Task Can_DeserialiserCompositionResponse_Success(string assetFile)
        {
            // arrange
            var options = Services.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync($"Ehr/Compositions/{assetFile}.json");

            // act
            var dataObject = JsonSerializer.Deserialize<Composition>(assetFileContent, options);
            var serialisedObject = JsonSerializer.Serialize(dataObject, options);

            // TODO : Got as far as Other Context - Items property. Might need a custom JsonConverter

            // assert
            Assert.NotNull(dataObject);
            Assert.NotNull(serialisedObject);
            Assert.NotEmpty(serialisedObject);

            // at least check the uid has been deserialised from the json. this also checks the "Root" extension
            // method works to extract just the guid part of the identifier from the Uid
            Assert.Equal(dataObject.Uid.Root().Value, assetFile);
        }
    }
}
