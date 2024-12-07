namespace Shellscripts.OpenEHR.Tests
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using Shellscripts.OpenEHR.Models.Definition;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class DefinitionTests : BaseTest
    {

        public DefinitionTests(ITestOutputHelper outputHelper) : base(outputHelper) { }
        

        [Fact]
        [Trait(name: "Category", value: "Unit Test")]
        public async Task Can_DeserialiseTemplateResponse_Success()
        {
            // arrange / act / assert
            int expectedCount = 39;
            string assetFileContent = await LoadAssetAsync("Definition/Template/TemplateList_ADL14.json");
            Assert.NotNull(assetFileContent);
            Assert.NotEmpty(assetFileContent);

            ADL14_TemplateListItem[] deserialisedObject = Array.Empty<ADL14_TemplateListItem>();
            deserialisedObject = JsonSerializer.Deserialize<ADL14_TemplateListItem[]>(assetFileContent);

            Assert.True(deserialisedObject.Count() == expectedCount, "Unexpected number of items in collection");
            foreach (var item in deserialisedObject)
            {
                Assert.NotNull(item.Concept);
                Assert.False(string.IsNullOrWhiteSpace(item.Concept));
            }
        }


    }
}
