namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using Shellscripts.OpenEHR.Tests.Context;
    using Shellscripts.OpenEHR.ViewModels;
    using Xunit;
    using Xunit.Abstractions;

    public class DefinitionTests : BaseTest
    {

        public DefinitionTests(ITestOutputHelper outputHelper, TestFixture testFixture) 
            : base(outputHelper, testFixture) { }


        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseTemplateListResponse_Success()
        {
            // arrange / act / assert
            int expectedCount = 39;
            string assetFileContent = await LoadAssetAsync("Definition/Template/TemplateList_ADL14.json");

            AdlTemplate_14[] deserialisedObject = Array.Empty<AdlTemplate_14>();
            deserialisedObject = JsonSerializer.Deserialize<AdlTemplate_14[]>(assetFileContent);

            Assert.True(deserialisedObject.Count() == expectedCount, "Unexpected number of items in collection");
            foreach (var item in deserialisedObject)
            {
                Assert.NotNull(item.Concept);
                Assert.False(string.IsNullOrWhiteSpace(item.Concept));
            }
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Unit")]
        public async Task Can_DeserialiseTemplateResponse_Success()
        {
            // arrange / act / assert
            string assetFileContent = await LoadAssetAsync("Definition/Template/Template_ADL14.json");

            AdlTemplate_14 deserialisedObject = null;
            deserialisedObject = JsonSerializer.Deserialize<AdlTemplate_14>(assetFileContent);

            Assert.NotNull(deserialisedObject.Concept);
            Assert.False(string.IsNullOrWhiteSpace(deserialisedObject.Concept));
        }
    }
}