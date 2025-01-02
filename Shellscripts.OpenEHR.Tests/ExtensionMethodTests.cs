namespace Shellscripts.OpenEHR.Tests
{
    using Shellscripts.OpenEHR.Extensions;
    using Shellscripts.OpenEHR.Models.BaseTypes;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;
    using static System.Net.Mime.MediaTypeNames;

    public class ExtensionMethodTests : BaseTest
    {
        public ExtensionMethodTests(ITestOutputHelper outputHelper, TestFixture testFixture)
            : base(outputHelper, testFixture) { }

        //[Fact]
        [Theory]
        [Trait(name: "TestCategory", value: "Unit")]
        [InlineData("6fa3ad70-9594-4a61-bf1b-4d114e723478::Local::001", "6fa3ad70-9594-4a61-bf1b-4d114e723478")]
        [InlineData("6fa3ad70-9594-4a61-bf1b-4d114e723478::Local", "6fa3ad70-9594-4a61-bf1b-4d114e723478")]
        [InlineData("6fa3ad70-9594-4a61-bf1b-4d114e723478", "6fa3ad70-9594-4a61-bf1b-4d114e723478")]
        [InlineData("", "")]
        public async Task Check_UidBasedId_RootMethod_Succeeds(string idValue, string expectedValue)
        {
            // Expected Behaviour
            // Returns the part to the left of the first '::' separator, if any,
            // or else the whole string.

            // arrange
            var sourceUid = new UidBasedId() { Value = idValue };

            // act
            var uid = sourceUid.Root(); // this "should" just be the Test Guid

            // assert
            OutputHelper.WriteLine($"IdValue\t: {idValue}\nExpected Value\t: {expectedValue}\nActual Value\t: {uid.Value}");
            Assert.Equal(expectedValue, uid.Value);
        }

        [Theory]        
        [Trait(name: "TestCategory", value: "Unit")]
        [InlineData("6fa3ad70-9594-4a61-bf1b-4d114e723478::Local::001", "Local::001")]
        [InlineData("6fa3ad70-9594-4a61-bf1b-4d114e723478::Local", "Local")]
        [InlineData("6fa3ad70-9594-4a61-bf1b-4d114e723478", "")]
        [InlineData("", "")]
        public async Task Check_UidBasedId_ExtensionMethod_Succeeds(string idValue, string expectedValue)
        {
            // Expected Behaviour
            // Returns the part to the right of the first '::' separator if any,
            // or else any empty String.

            // arrange
            var sourceUid = new UidBasedId() { Value = idValue };

            // act
            var ext = sourceUid.Extension();

            // assert
            OutputHelper.WriteLine($"IdValue\t: {idValue}\nExpected Value\t: {expectedValue}\nActual Value\t: {ext}");
            Assert.Equal(expectedValue, ext);
        }

    }
}
