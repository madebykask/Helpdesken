using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using ExtendedCase.HelpdeskApiClient.Interfaces;

namespace ExtendedCase.WebApi.Infrastructure.Filters
{
    public class TokensInterceptionFilter : IAutofacActionFilter
    {
        private readonly IApiTokenProvider _tokenProvider;

        #region ctor()

        public TokensInterceptionFilter(IApiTokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        #endregion

        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            //Debugger.Launch();

            var authToken = GetCustomHeaderValue("helpdeskAuthToken", actionContext.Request.Headers);
            _tokenProvider.SetToken(authToken);

            var refreshToken = GetCustomHeaderValue("helpdeskRefreshToken", actionContext.Request.Headers);
            _tokenProvider.SetRefreshToken(refreshToken);
            

            return Task.FromResult(0);

        }

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        #region Helpder Methods

        private string GetCustomHeaderValue(string key, HttpRequestHeaders requestHeaders)
        {
            string value = string.Empty;

            IEnumerable<string> values = null;
            if (requestHeaders.TryGetValues(key, out values))
            {
                value = values.FirstOrDefault();
            }

            return value;
        }

        #endregion
    }
}