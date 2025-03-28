﻿namespace Shellscripts.OpenEHR.Rest
{
    using System.Text;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;
    using Shellscripts.OpenEHR.Extensions;
    using Shellscripts.OpenEHR.Models.Ehr;
    using Shellscripts.OpenEHR.Models.PlatformServiceModel;

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
        public async Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, CancellationToken?, Task<TResult>> action, CancellationToken? cancellationToken = default)
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
        public async Task<TResult?> GetAsync<TResult>(string url, CancellationToken? cancellationToken)
        {
            return await ExecuteAsync(async (client, cancellationToken) =>
            {
                try
                {
                    var responseMessage = await client.GetAsync(url, cancellationToken ?? CancellationToken.None);

                    responseMessage.EnsureSuccessStatusCode();

                    var stringContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken ?? CancellationToken.None);

                    if (typeof(TResult) == typeof(String))
                    {
                        return (TResult)Convert.ChangeType(stringContent, typeof(TResult));
                    }

                    return JsonSerializer.Deserialize<TResult>(stringContent, _options);
                }
                catch (HttpRequestException hEx) when (hEx.StatusCode is System.Net.HttpStatusCode.BadRequest)
                {
                    _logger.LogError(hEx.Message, hEx);
                    throw;
                }
                catch (HttpRequestException hEx) when (hEx.StatusCode is System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning(hEx.Message, hEx);
                    throw;
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
        public async Task<string?> PostAsync(string url, string data, CancellationToken? cancellationToken)
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

                    var responseMessage = await client.PostAsync(url, httpContent, cancellationToken ?? CancellationToken.None);
                    responseMessage.EnsureSuccessStatusCode();

                    var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken ?? CancellationToken.None);
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
        public async Task<string?> PostAsync<TModelObject>(string url, TModelObject data, CancellationToken? cancellationToken)
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

        public async Task<string?> PutAsync<TModelObject>(string url, TModelObject data, CancellationToken? cancellationToken)
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

                return await PutAsync(url, serialisedData, cancellationToken);
            });
        }

        /// <summary>
        /// Put a string to the target Ehr Server
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<string?> PutAsync(string url, string data, CancellationToken? cancellationToken)
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

                    var responseMessage = await client.PutAsync(url, httpContent, cancellationToken ?? CancellationToken.None);
                    responseMessage.EnsureSuccessStatusCode();

                    var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken ?? CancellationToken.None);
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

        public async Task<IEnumerable<TR>> QueryAsync<TR>(string query, CancellationToken? cancellationToken)
        {
            string url = $"/query/aql";

            var query_result = await PostAsync(url, new { q = query }, cancellationToken);
            
            if (query_result is null)
                return [];

            var serialised_result = JsonSerializer.Deserialize<ResultSet>(query_result, _options);

            if (serialised_result != null && serialised_result.Rows.Length != 0)
            {
                var row_values = serialised_result
                    .Rows
                    .SelectMany(r => r.Values)
                    .Where(r => r is not null)
                    .Select(o => JsonSerializer.Deserialize<TR>(o.ToString() ?? string.Empty, _options))
                    ;

                return row_values;
            }
            else
            {
                // Couldn't serialise the ResultSet ?
                return [];
            }

            
        }

        #endregion
    }
}