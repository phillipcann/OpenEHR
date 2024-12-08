namespace Shellscripts.OpenEHR.Rest
{
    using Microsoft.Extensions.Logging;

    public class EhrClient : IEhrClient
    {
        private ILogger<EhrClient> _logger { get; set; }
        private HttpClient _client { get; set; }

        /// <summary>
        /// EhrClient
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="client"></param>
        public EhrClient(ILogger<EhrClient> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
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
                // TODO : This will be a use case for re-negotiating the credentials / token

                _logger.LogWarning(hEx, hEx.Message);
                return await action(_client, cancellationToken);
            }
        }




        #endregion
    }

    public class UriAppendingHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return base.SendAsync(request, cancellationToken);
        }
    }
}