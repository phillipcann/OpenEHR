namespace Shellscripts.OpenEHR.Tests
{
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Shellscripts.OpenEHR.Extensions;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Models.DataStructures;
    using Shellscripts.OpenEHR.Models.CommonInformation;
    using Shellscripts.OpenEHR.Models.DataTypes;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class SerialisationTests : BaseTest
    {

        public SerialisationTests(ITestOutputHelper outputHelper, TestFixture testFixture)
            : base(outputHelper, testFixture) { }


        [Fact]
        [Trait(name: "Category", value: "Unit Test")]
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
                    IsModifiable = true,
                    IsQueryable = true
                },
                TimeCreated = new DvDateTime() { Value = "2024-08-30T21:45:33.389606Z" }
            };

            string expected_json = await LoadAssetAsync($"Serialisation/Ehr.json");
            var serialiserOptions = Services.GetRequiredService<JsonSerializerOptions>();

            // act
            var actual_json = JsonSerializer.Serialize(mock_ehr, serialiserOptions);

            // assert
            Assert.NotNull(actual_json);
            Assert.NotEmpty(actual_json);
            Assert.True(await AreJsonStringsEqualAsync(actual_json, expected_json));
        }

    }
}
