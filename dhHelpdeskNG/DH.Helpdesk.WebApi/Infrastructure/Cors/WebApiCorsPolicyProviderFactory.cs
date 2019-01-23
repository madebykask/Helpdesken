using System.Net.Http;
using System.Web.Http.Cors;

namespace DH.Helpdesk.WebApi.Infrastructure.Cors
{
    public class WebApiCorsPolicyProviderFactory : ICorsPolicyProviderFactory
    {
        private readonly ICorsPolicyProvider _corsPolicyProvider;

        /// <summary>
        /// Cors policy provider factory
        /// </summary>
        public WebApiCorsPolicyProviderFactory()
        {
            _corsPolicyProvider = new WebApiCorsPolicyProvider();
        }

        public ICorsPolicyProvider GetCorsPolicyProvider(HttpRequestMessage request)
        {
            //you may have different policy providers handling request based on some rules
            return _corsPolicyProvider;
        }
    }
}