namespace Shellscripts.OpenEHR.Tests
{
    using System.Net;
    using Microsoft.Extensions.DependencyInjection;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Rest;
    using Shellscripts.OpenEHR.Tests.Context;
    using Xunit;
    using Xunit.Abstractions;

    public class RestClientTests : BaseTest
    {
        public RestClientTests(ITestOutputHelper outputHelper, TestFixture testFixture) 
            : base(outputHelper, testFixture) { }

        [Fact]
        [Trait(name: "TestCategory", value: "Integration")]
        public async Task Test_GetAsync_UrlNotFound_ThrowsHttpRequestException_Success()
        {
            // arrange
            var client = Services?.GetRequiredService<IEhrClient>();
            var token = CancellationToken.None;
            Assert.NotNull(client);

            // act
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await client.GetAsync<Ehr>("ehr/invalid", token);
            });

            Assert.Contains($"{(int)HttpStatusCode.NotFound}", exception.Message);
        }

        [Theory(DisplayName = "Test PostAsync Throws Expected Exceptions")]
        [Trait(name: "TestCategory", value: "Integration")]
        [ClassData(typeof(EhrClientPostData))]
        public async Task Test_PostAsync_ThrowsException_Success(string url, object? data, Type expectedException, string expectedExceptionMessage)
        {
            // arrange
            var client = Services?.GetRequiredService<IEhrClient>();
            var token = CancellationToken.None;
            
            Assert.NotNull(client);

            // act
            var exception = await Assert.ThrowsAsync(expectedException, async () =>
            {
                await client.PostAsync(url, data, token);
            });

            // assert
            Assert.Contains(expectedExceptionMessage, exception.Message);
        }
    }


}
