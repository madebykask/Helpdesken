using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace DH.Helpdesk.WebApi.Infrastructure.Cors
{
    public class WebApiCorsPolicyProvider : ICorsPolicyProvider
    {
        public Task<CorsPolicy> GetCorsPolicyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var absolutePath = request.RequestUri.AbsolutePath;
            if (Regex.IsMatch(absolutePath, "/api/case/({)?[0-9A-Za-z-]+(})?/file", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline) ||
                Regex.IsMatch(absolutePath, "/api/case/({)?[0-9A-Za-z-]+(})?/logfile", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline))
            {
                return GetCorsPolicyWithCredentials();
            }

            var policy = GetCorsPolicy();
            return Task.FromResult(policy);
        }

        #region Policy Methods

        private CorsPolicy GetCorsPolicy()
        {
            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                SupportsCredentials = false,
                PreflightMaxAge = 600
            };
            return policy;
        }

        private async Task<CorsPolicy> GetCorsPolicyWithCredentials()
        {
            //todo: add specific origins?
            var policy = new CorsPolicy()
            {
                AllowAnyHeader = true,
                AllowAnyMethod = true,
                AllowAnyOrigin = true,
                SupportsCredentials = true,
                PreflightMaxAge = 600
            };
            return policy;
        }

        #endregion
    }
}