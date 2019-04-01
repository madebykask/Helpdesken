using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;

namespace ExtendedCase.HelpdeskApiClient.Handlers
{
    public class HttpClientDiagnosticHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public HttpClientDiagnosticHandler(ILogger logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Log($"Request: {request.RequestUri}");

            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var msg = $"Response received: {request.RequestUri}\t{(int)response.StatusCode}\t{response.Headers.Date}";
                Log(msg);

                var content = await response.Content.ReadAsStringAsync();
                Log($"Response: {content}");
            }
            return response;
        }

        private void Log(string msg)
        {
            _logger.Debug($"HttpClient: {msg}");
        }
    }
}