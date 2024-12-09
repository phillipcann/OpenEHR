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

        [Fact]
        [Trait(name: "Category", value: "Unit Test")]
        public async Task Can_DeserialiserCompositionResponse_Success()
        {
            // arrange
            var options = Services.GetRequiredService<JsonSerializerOptions>();
            string assetFileContent = await LoadAssetAsync("Ehr/GetCompositionResponse.json");

            // act
            var dataObject = JsonSerializer.Deserialize<Composition>(assetFileContent, options);
            var serialisedObject = JsonSerializer.Serialize(dataObject, options);

            // assert
            Assert.NotNull(dataObject);
            Assert.NotNull(serialisedObject);
            Assert.NotEmpty(serialisedObject);
        }
    }
}
