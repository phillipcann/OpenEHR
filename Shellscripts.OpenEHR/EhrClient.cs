namespace Shellscripts.OpenEHR
{
    using Microsoft.Extensions.Logging;

    public class EhrClient : HttpClient
    {
        private ILogger<EhrClient> _logger { get; set; }

        public EhrClient(ILogger<EhrClient> logger)
        {
            _logger = logger;            
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
        public async Task<TResult> ExecuteAsync<TResult>(Func<HttpClient, CancellationToken, Task<TResult>> action, CancellationToken cancellationToken = default)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (HttpRequestException hEx) when (hEx.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // This will be a use case for re-negotiating the credentials / token

                _logger.LogWarning(hEx, hEx.Message);
                return await action(this, cancellationToken);
            }            
        }

        #endregion
    }
}