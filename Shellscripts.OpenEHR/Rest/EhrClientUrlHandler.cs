namespace Shellscripts.OpenEHR.Rest
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class EhrClientUrlHandler : DelegatingHandler
    {
        private readonly string _systemUri;
        private readonly string _baseUrl;
        private readonly ILogger<EhrClientUrlHandler> _logger;

        public EhrClientUrlHandler(IConfiguration configuration, ILogger<EhrClientUrlHandler> logger) 
        {
            var ehrClientSection = configuration.GetSection("HttpClients");

            _systemUri = ehrClientSection.GetValue<string>("EhrClient:SystemUri", string.Empty).TrimStart('/');
            _baseUrl = ehrClientSection.GetValue<string>("EhrClient:BaseUrl", string.Empty);
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_systemUri))
            {
                var extractedBaseUrl = request.RequestUri.GetLeftPart(UriPartial.Authority);
                var modifiedUri = new Uri($"{extractedBaseUrl}/{_systemUri}/{request.RequestUri.PathAndQuery.TrimStart('/')}");

                _logger.LogInformation($"EhrClientUrlHandler :: Amended Url: {modifiedUri.ToString()}");

                request.RequestUri = modifiedUri;
            }            
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
