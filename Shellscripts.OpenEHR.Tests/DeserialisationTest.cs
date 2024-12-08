namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Serialisation.Converters;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class DeserialisationTest : BaseTest
    {
        public DeserialisationTest(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        [Trait(name: "Category", value: "Unit Test")]
        public async Task Can_DeserialiseVersionedEhrStatusResponse_Success()
        {
            // arrange
            string assetFileContent = await LoadAssetAsync("Ehr/VersionedEhrStatusResponse.json");
            var options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new DvDateTimeConverter());

            // act
            var dataObject = JsonSerializer.Deserialize<VersionedEhrStatus>(assetFileContent, options);
            var serialisedObject = JsonSerializer.Serialize(dataObject, options);

            // assert
            Assert.NotNull(dataObject);
            Assert.NotNull(serialisedObject);
            Assert.NotEmpty(serialisedObject);
        }
    }


}
