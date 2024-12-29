namespace Shellscripts.OpenEHR.Rest
{
    using System.Text;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Models.Ehr;

    public class EhrClient : IEhrClient
    {
        private readonly ILogger<EhrClient> _logger;

        private readonly JsonSerializerOptions _options;
        private HttpClient _client { get; set; }

        /// <summary>
        /// EhrClient
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="client"></param>
        /// <param name="options"></param>
        public EhrClient(ILogger<EhrClient> logger, HttpClient client, JsonSerializerOptions options)
        {
            _logger = logger;
            _client = client;
            _options = options;
        }


        #region Public Methods

        /// <summary>
        /// Custom Execute Method which will allow for auto re-negotiation of authorisation headers
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>        
        public async Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, CancellationToken, Task<TResult>> action, CancellationToken cancellationToken = default)
        {
            try
            {
                return await action(_client, cancellationToken);
            }
            catch (HttpRequestException hEx) when (hEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning(hEx, hEx.Message);

                // TODO : This will be a use case for re-negotiating the credentials / token
                return await action(_client, cancellationToken);
            }
        }

        #region Candidates for moving to a Repository class

        public async Task<Ehr?> GetEhrAsync(string ehrId, CancellationToken cancellationToken)
        {
            string url = $"/ehr/{ehrId}";
            return await GetAsync<Ehr>(url, cancellationToken);
        }

        public async Task<VersionedEhrStatus?> GetVersionedEhrStatusAsync(string ehrId, CancellationToken cancellationToken)
        {
            string url = $"/ehr/{ehrId}/versioned_ehr_status";
            return await GetAsync<VersionedEhrStatus>(url, cancellationToken);
        }

        public async Task<Composition?> GetCompositionAsync(string ehrId, string compositionId, CancellationToken cancellationToken)
        {
            string url = $"/ehr/{ehrId}/composition/{compositionId}";
            return await GetAsync<Composition>(url, cancellationToken);
        }

        public async Task<VersionedComposition?> GetVersionedCompositionAsync(string ehrId, string compositionId, CancellationToken cancellationToken)
        {
            string url = $"/ehr/{ehrId}/versioned_composition/{compositionId}";
            return await GetAsync<VersionedComposition>(url, cancellationToken);
        }

        #endregion


        #endregion

        #region Public Methods

        public async Task<TResult?> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        {
            return await ExecuteAsync(async (client, cancellationToken) =>
            {
                try
                {
                    var responseMessage = await client.GetAsync(url, cancellationToken);

                    responseMessage.EnsureSuccessStatusCode();

                    var stringContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                    return JsonSerializer.Deserialize<TResult>(stringContent, _options);
                }
                catch (HttpRequestException hEx) when (hEx.StatusCode is System.Net.HttpStatusCode.BadRequest)
                {
                    _logger.LogError(hEx.Message, hEx);

                    return default;
                }
                catch (JsonException jEx)
                {
                    _logger.LogError(jEx.Message, jEx);
                    throw;
                }

            }, cancellationToken);
        }

        public async Task<TResult?> PostAsync<TModelObject, TResult>(string url, TModelObject data, CancellationToken cancellationToken)
        {
            return await ExecuteAsync(async (client, cancellationToken) =>
            {
                var serialisedData = JsonSerializer.Serialize(data, _options);
                var httpContent = new StringContent(serialisedData, Encoding.UTF8, "application/json");

                try
                {
                    var responseMessage = await client.PostAsync(url, httpContent, cancellationToken);
                    responseMessage.EnsureSuccessStatusCode();

                    var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                    var response = JsonSerializer.Deserialize<TResult>(responseContent, _options);

                    return response;
                }
                catch (HttpRequestException hEx) when (hEx.StatusCode is System.Net.HttpStatusCode.BadRequest)
                {
                    _logger.LogError(hEx.Message, hEx);

                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);

                    throw;
                }
            });
        }

        #endregion

    }
}