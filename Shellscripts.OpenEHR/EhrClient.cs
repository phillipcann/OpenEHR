namespace Shellscripts.OpenEHR
{
    using Microsoft.Extensions.Logging;

    public class EhrClient
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
        /// <exception cref="NotImplementedException"></exception>
        //public async Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, HttpRequestMessage, CancellationToken, Task<TResult>> action,
        //    HttpRequestMessage request,
        //    CancellationToken cancellationToken = default)
        //{
        //    try
        //    {
        //        return await action(_client, request, cancellationToken);
        //    }
        //    catch (HttpRequestException hEx) when (hEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //    {
        //        // This will be a use case for re-negotiating the credentials / token

        //        _logger.LogWarning(hEx, hEx.Message);
        //        return await action(_client, request, cancellationToken);
        //    }            

        //    switch (request.)
        //    {
        //        case HttpMethod:

        //            break;
        //    }
        //}

        


        #endregion
    }
}