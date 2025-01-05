namespace Shellscripts.OpenEHR.Rest
{
    using System.Text;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Extensions;
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

        /// <summary>
        /// Retrieve a resource
        /// </summary>
        /// <typeparam name="TResult">The type of reference model item we are retrieving that we wish to deserialise the result as</typeparam>
        /// <param name="url"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResult?> GetAsync<TResult>(string url, CancellationToken cancellationToken)
        {
            return await ExecuteAsync(async (client, cancellationToken) =>
            {
                try
                {
                    var responseMessage = await client.GetAsync(url, cancellationToken);

                    responseMessage.EnsureSuccessStatusCode();

                    var stringContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);

                    if (typeof(TResult) == typeof(String))
                    {
                        return (TResult)Convert.ChangeType(stringContent, typeof(TResult));
                    }

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

        /// <summary>
        /// Post a string to the target Ehr Server
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<string?> PostAsync(string url, string data, CancellationToken cancellationToken)
        {
            if (data == null)
            {
                var nullDataErrorMessage = $"{nameof(data)} cannot be null";

                _logger.LogError(nullDataErrorMessage);
                throw new NullReferenceException(nullDataErrorMessage);
            }

            return await ExecuteAsync(async (client, cancellationToken) =>
            {
                try
                {
                    var httpContent = new StringContent(data, Encoding.UTF8, "application/json");

                    var responseMessage = await client.PostAsync(url, httpContent, cancellationToken);
                    responseMessage.EnsureSuccessStatusCode();

                    var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
                    var responseHeaders = responseMessage.Headers;

                    if (responseHeaders.TryGetValues("eTag", out var eTags))
                    {
                        return string.Join(',', eTags);
                    }
                    else if (!string.IsNullOrWhiteSpace(responseContent))
                    {
                        return responseContent;
                    }

                    // TODO : Implement the return object / data

                    //var response = JsonSerializer.Deserialize(responseContent, _options);
                    return string.Empty;
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

        /// <summary>
        /// Post a strongly typed Ehr Reference Model item as Post Data to the target Ehr Server
        /// </summary>
        /// <typeparam name="TModelObject"></typeparam>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<string?> PostAsync<TModelObject>(string url, TModelObject data, CancellationToken cancellationToken)
        {
            if (data == null)
            {
                var nullDataErrorMessage = $"{nameof(data)} cannot be null";

                _logger.LogError(nullDataErrorMessage);
                throw new NullReferenceException(nullDataErrorMessage);
            }

            return await ExecuteAsync(async (client, cancellationToken) =>
            {
                var serialisedData = data.IsAnonymousType()
                    ? JsonSerializer.Serialize(data, data.GetType())
                    : JsonSerializer.Serialize(data, data.GetType(), _options);

                return await PostAsync(url, serialisedData, cancellationToken);
            });
        }


        #region IEhrServiceModel Implementation - Candidates for shifting to a Repository Type of class?

        public async Task<Ehr?> GetEhrAsync(string ehrId, CancellationToken cancellationToken)
        {
            string url = $"/ehr/{ehrId}";
            return await GetAsync<Ehr>(url, cancellationToken);
        }

        public async Task<Ehr?> GetEhrAsync(string subject_namespace, string subject_id, CancellationToken cancellationToken)
        {
            string url = $"/ehr?subject_id={subject_id}&subject_namespace={subject_namespace}";
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

    }
}