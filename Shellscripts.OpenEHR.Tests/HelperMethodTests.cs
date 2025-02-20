namespace Shellscripts.OpenEHR.Tests
{
    using System;
    using System.Threading.Tasks;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;
    

    public class HelperMethodTests : BaseTest
    {
        public HelperMethodTests(ITestOutputHelper outputHelper, TestFixture testFixture)
            : base(outputHelper, testFixture) { }

        //readonly string test_json = "{ \"user\": { \"name\": \"John\", \"age\": 30, \"deceased\": true, \"weight\": 160.4523, \"height\": null  } }";
        readonly string test_json = """
        { 
            "user": { 
                "name": "John", 
                "age": 30, 
                "deceased": true, 
                "weight": 160.4523, 
                "height": null  
            },
            "address": [
                { "number": 1 },
                { "number": 2 },
                { "number": 3 }
            ]
        }
        """;


        [Theory]
        [Trait(name: "TestCategory", value: "Unit")]
        [InlineData("$.user.name", "John")]
        [InlineData("$.user.age", 30)]
        [InlineData("$.user.weight", 160.4523)]
        [InlineData("$.user.deceased", true)]
        [InlineData("$.user.height", null)]
        [InlineData("$.address[0].number", 1)]
        [InlineData("$.address[1].number", 2)]
        [InlineData("$.address[2].number", 3)]
        public async Task Can_AssertTypeSafeValue_Success(string json_path, object? expectedValue)
        {
            await Task.Run(() => HelperMethods.AsssertJsonValueEquality(test_json, json_path, expectedValue));
        }


        [Theory]
        [Trait(name: "TestCategory", value: "Unit")]
        [InlineData("$.user.age", "30", typeof(InvalidOperationException), "Type mismatch")]
        [InlineData("$.user.age", 31, typeof(InvalidOperationException), "Value mismatch")]
        [InlineData("$.user.doesntexist", 31, typeof(ArgumentException), "No match found")]
        [InlineData("$.user.height", 175, typeof(InvalidOperationException), "Type mismatch")]
        [InlineData(null, 30, typeof(ArgumentException), "JSONPath cannot be null")]
        public async Task Does_AssertThrowExpectedExceptions_Success(string? json_path, object? expectedValue, Type expectedException, string expectedExceptionMessageStart)
        {
            var exception = await Assert.ThrowsAsync(expectedException, async () =>
            {
                await Task.Run(() => HelperMethods.AsssertJsonValueEquality(test_json, json_path, expectedValue));
            });

            Assert.StartsWith(expectedExceptionMessageStart, exception.Message);
        }

    }
}
