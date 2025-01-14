namespace Shellscripts.OpenEHR.Tests
{
    using System.Buffers.Text;
    using System.Net;
    using System.Text.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Models.PlatformServiceModel;
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


        [Fact]
        [Trait(name: "TestCategory", value: "Integration")]
        public async Task Test_GetAsync_QueryApi_Succeeds()
        {
            // arrange
            var client = Services?.GetRequiredService<IEhrClient>();
            var token = CancellationToken.None;

            var aqlString = "SELECT e/ehr_id/value FROM EHR e";
            var queryUrl = $"query/aql?q={aqlString}";
            Assert.NotNull(client);

            // act
            var response = await client.GetAsync<string>(queryUrl, token);

            // assert
            Assert.NotNull(response);

            // response should be json parseable            
            try
            {
                var parsedQueryResponse = JsonDocument.Parse(response);                
                Assert.NotNull(parsedQueryResponse);
                Assert.True(parsedQueryResponse.RootElement.ValueKind == JsonValueKind.Object);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        [Trait(name: "TestCategory", value: "Integration")]
        public async Task Test_PostAsync_QueryApi_Succeeds()
        {
            // arrange
            var client = Services?.GetRequiredService<IEhrClient>();
            var options = Services?.GetRequiredService<JsonSerializerOptions>();
            var token = CancellationToken.None;

            var aqlString = new {
                q = "SELECT e/ehr_id/value FROM EHR e",
                offset = 0,
                fetch = 15,
                query_parameters = new { }
            };

            var queryUrl = $"query/aql";
            Assert.NotNull(client);

            // act
            var response = await client.PostAsync(queryUrl, aqlString, token);

            // assert
            Assert.NotNull(response);

            // response should be serialisable to ResultSet type
            try
            {
                var parsedQueryResponse = JsonSerializer.Deserialize<ResultSet>(response, options);
                Assert.NotNull(parsedQueryResponse);
                Assert.Equal(aqlString.q, parsedQueryResponse.Query);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
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

        [Fact]
        [Trait(name: "TestCategory", value: "Integration")]
        public async Task Test_PostAsync_QueryAql_Success()
        {
            // TODO : This test will only succeed if the sandbox has results to return so it's not an ideal test.

            // arrange
            var client = Services?.GetRequiredService<IEhrClient>();
            var token = CancellationToken.None;
            var request_body = new { q = "SELECT c FROM COMPOSITION c WHERE c/archetype_details/archetype_id/value='openEHR-EHR-COMPOSITION.report-result.v1' LIMIT 2" };

            // act
            var query_response = await client.PostQueryAqlAsync<Composition>(request_body, token);

            // assert
            Assert.NotNull(query_response);
            Assert.True(query_response.Count() > 0);
        }
        
    }


}
