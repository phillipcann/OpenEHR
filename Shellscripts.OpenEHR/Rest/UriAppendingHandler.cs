namespace Shellscripts.OpenEHR.Rest
{
    // TODO : This "might" be a reasonable solution to appending a Uri segement to the BaseUrl. Investigate
    public class UriAppendingHandler : DelegatingHandler
    {
        private readonly string _requiredSegment;

        public UriAppendingHandler(string requiredSegment)
        {
            _requiredSegment = requiredSegment;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // TODO : This isnt right. We ALWAYS want to append the segment to the Base URL
            // TODO : When this code IS always run, presently it puts the segment at the beginning of the URL breaking the function
            if (!request.RequestUri.IsAbsoluteUri)
            {
                // Ensure the segment is appended to the relative request URI
                request.RequestUri = new Uri(_requiredSegment.TrimEnd('/') + "/" + request.RequestUri.ToString().TrimStart('/'), UriKind.Relative);
            }

            return await base.SendAsync(request, cancellationToken);
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.RequestUri.IsAbsoluteUri)
            {
                // Ensure the segment is appended to the relative request URI
                request.RequestUri = new Uri(_requiredSegment.TrimEnd('/') + "/" + request.RequestUri.ToString().TrimStart('/'), UriKind.Relative);
            }

            return base.Send(request, cancellationToken);
        }
    }
}