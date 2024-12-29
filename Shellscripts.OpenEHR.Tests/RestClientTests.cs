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
        [Trait(name: "Category", value: "Integration Test")]
        public async Task Test_UrlNotFound_ThrowsHttpRequestException_Success()
        {
            // arrange
            var client = base.Services.GetRequiredService<IEhrClient>();

            // act
            var token = CancellationToken.None;

            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
            {
                await client.GetAsync<Ehr>("ehr/invalid", token);
            });

            Assert.Contains($"{(int)HttpStatusCode.NotFound}", exception.Message);
        }
    }

}
