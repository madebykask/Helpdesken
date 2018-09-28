using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.Controllers;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Services.Services;
using Microsoft.AspNet.Identity;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Filters
{
    public class UserCasePermissionsFilter: AuthorizationFilterBase, IAutofacAuthorizationFilter
    {
        private readonly IUserService _userService;

        public UserCasePermissionsFilter(IUserService userService)
        {
            _userService = userService;
        }
        protected override void AuthorizeRequest(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (principal != null)
            {
                var userId = principal.Identity.GetUserId().ToInt();
                var user = _userService.GetUserOverview(userId);
                if (user == null)
                {
                    actionContext.Response = CreateForbidddenResponse(actionContext.Request, "User is not authorized to access the case");
                }

                var caseId = GetCaseIdFromRequest(actionContext);
                if (caseId <= 0)
                {
                    actionContext.Response = CreateBadResponse(actionContext.Request); 
                    return;
                }

                var isAuthorised = _userService.VerifyUserCasePermissions(user, caseId);
                if (!isAuthorised)
                {
                    actionContext.Response = CreateForbidddenResponse(actionContext.Request, "User is not authorized to access the case");
                }
            }
            else
            {
                actionContext.Response = CreateNotAuthenticatedResponse(actionContext.Request);
            }
        }

        private int GetCaseIdFromRequest(HttpActionContext actionContext)
        {
            var paramName = "id";
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<CheckUserCasePermissionsAttribute>(false).FirstOrDefault();
            if (!string.IsNullOrEmpty(attr?.CaseIdParamName))
                paramName = attr.CaseIdParamName;

            var paramValue = string.Empty;
            var caseId = 0;

            if (actionContext.RequestContext.RouteData.Values.Keys.Contains(paramName))
            {
                paramValue = (actionContext.RequestContext.RouteData.Values[paramName] ?? string.Empty).ToString();
            }

            if (string.IsNullOrEmpty(paramValue))
            {
                paramValue = GetQueryStringParam(actionContext.Request, paramName);
            }

            if (Int32.TryParse(paramValue, out caseId))
                return caseId;

            return 0;
        }

        protected override bool IgnoreRequest(HttpActionContext actionContext)
        {
            //ignore if action doesn't have attribute
            var hasAttr = CheckAttribute<CheckUserCasePermissionsAttribute>(actionContext, AttributeCheckScope.Action);
            if (!hasAttr)
                return true;
            return false;
        }
    }
}