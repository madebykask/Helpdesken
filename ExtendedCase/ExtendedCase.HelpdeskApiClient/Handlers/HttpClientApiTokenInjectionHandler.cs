using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ExtendedCase.HelpdeskApiClient.Interfaces;

namespace ExtendedCase.HelpdeskApiClient.Handlers
{
    public class HttpClientApiTokenInjectionHandler : DelegatingHandler
    {
        private readonly IApiTokenProvider _apiTokenProvider;

        #region ctor()

        public HttpClientApiTokenInjectionHandler(IApiTokenProvider apiTokenProvider)
        {
            _apiTokenProvider = apiTokenProvider;
        }

        #endregion

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _apiTokenProvider.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}