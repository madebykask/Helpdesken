using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac.Integration.WebApi;

namespace DH.Helpdesk.WebApi.Infrastructure.Filters
{

    public abstract class AuthorizationFilterBase : IAutofacAuthorizationFilter
    {
        #region AttributeCheckScope

        [Flags]
        public enum AttributeCheckScope : uint
        {
            Controller = 1 << 0,
            Action  = 1 << 1,
            All = ~0u
        }

        #endregion

        #region IAutofacAuthorizationFilter implementation

        public Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (IgnoreRequest(actionContext))
                return Task.FromResult(0);

            if (!VerifyUser(actionContext))
                return Task.FromResult(0);

            AuthorizeRequest(actionContext);

            return Task.FromResult(0);
        }

        #endregion

        #region Virtual Methods

        protected abstract void AuthorizeRequest(HttpActionContext actionContext);

        protected virtual bool VerifyUser(HttpActionContext actionContext)
        {
            var userIdentity = actionContext.ControllerContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                actionContext.Response = CreateNotAuthenticatedResponse(actionContext.Request);
                return false;
            }
        }

        protected virtual bool IgnoreRequest(HttpActionContext actionContext)
        {
            if (CheckAttribute<AllowAnonymousAttribute>(actionContext))
                return true;

            return false;
        }

        #endregion

        #region Protected Methods

        protected bool CheckAttribute<TAttribute>(HttpActionContext actionContext, AttributeCheckScope attributeScope = AttributeCheckScope.All) where TAttribute : Attribute
        {
            var hasAttr = false;
            if ((attributeScope & AttributeCheckScope.Action) == AttributeCheckScope.Action)
            {
                hasAttr = actionContext.ActionDescriptor.GetCustomAttributes<TAttribute>(false).Any();
                if (hasAttr)
                    return true;
            }

            if ((attributeScope & AttributeCheckScope.Controller) == AttributeCheckScope.Controller)
            {
                hasAttr = actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<TAttribute>(false).Any();
                if (hasAttr)
                    return true;
            }

            return false;
        }

        protected HttpResponseMessage CreateBadResponse(HttpRequestMessage request, string error = null)
        {
            var msg = error ?? "Unknown server error";
            var response = request.CreateErrorResponse(HttpStatusCode.BadRequest, msg);
            return response;
        }

        protected HttpResponseMessage CreateForbidddenResponse(HttpRequestMessage request, string message = null)
        {
            var msg = message ?? "User is not authorized";
            return request.CreateErrorResponse(HttpStatusCode.Forbidden, msg);
        }

        protected HttpResponseMessage CreateNotAuthorizedResponse(HttpRequestMessage request, string message = null)
        {
            var msg = message ?? "User is not authorized";
            return request.CreateErrorResponse(HttpStatusCode.Forbidden, msg);
        }

        protected HttpResponseMessage CreateNotAuthenticatedResponse(HttpRequestMessage request, string message = null)
        {
            var msg = message ?? "User is not authenticated";
            return request.CreateErrorResponse(HttpStatusCode.Unauthorized, msg);
        }

        protected string GetQueryStringParam(HttpRequestMessage requestMessage, string paramName)
        {
            var queryParams = requestMessage.GetQueryNameValuePairs();
            var param = queryParams.FirstOrDefault(x => x.Key.Equals(paramName, StringComparison.OrdinalIgnoreCase));
            // no null check requeired since result is a struct        
            return param.Value;
        }

        #endregion
    }
}