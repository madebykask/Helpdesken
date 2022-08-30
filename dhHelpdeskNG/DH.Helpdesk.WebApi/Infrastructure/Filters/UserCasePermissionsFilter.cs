using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.Controllers;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
using DH.Helpdesk.Services.Services;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace DH.Helpdesk.WebApi.Infrastructure.Filters
{
    public class UserCasePermissionsFilter: AuthorizationFilterBase
    {
        private readonly IUserService _userService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;

        public UserCasePermissionsFilter(IUserService userService, IUserPermissionsChecker userPermissionsChecker)
        {
            _userService = userService;
            _userPermissionsChecker = userPermissionsChecker;
        }

        protected override void AuthorizeRequest(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
            if (principal != null)
            {
                var userId = principal.Identity.GetUserId().ToInt();
                var user = _userService.GetUser(userId);
                if (user == null)
                {
                    actionContext.Response = CreateForbidddenResponse(actionContext.Request, "User is not authorized to accesss");
                    return;
                }

                var hasCasePermissionAttr = CheckAttribute<CheckUserCasePermissionsAttribute>(actionContext, AttributeCheckScope.Action);
                if (hasCasePermissionAttr)
                {
                    var caseId = GetCaseIdFromRequest(actionContext);
                    if (caseId <= 0)
                    {
                        // actionContext.Response = CreateBadResponse(actionContext.Request);
                        return;
                    }

                    var isAuthorised = _userService.VerifyUserCasePermissions(UsersMapper.MapToOverview(user), caseId);
                    if (!isAuthorised)
                    {
                        actionContext.Response = CreateForbidddenResponse(actionContext.Request, "User is not authorized to access the case");
                        return;
                    }
                }

                var hasPermissionAttr = CheckAttribute<CheckUserPermissionsAttribute>(actionContext, AttributeCheckScope.Action);
                if (hasPermissionAttr)
                {
                    if (!HasUserPermissions(actionContext, user))
                    {
                        actionContext.Response = CreateForbidddenResponse(actionContext.Request, "User has no permissions to access action");
                        return;
                    }
                }
            }
            else
            {
                actionContext.Response = CreateNotAuthenticatedResponse(actionContext.Request);
            }
        }

        private bool HasUserPermissions(HttpActionContext actionContext, User user)
        {
            var attr = actionContext.ActionDescriptor.GetCustomAttributes<CheckUserPermissionsAttribute>(false).FirstOrDefault();
            if (attr?.UserPermissions == null || attr?.UserPermissions?.Length == 0)
                return false;

            return _userPermissionsChecker.UserHasAllPermissions(user, attr.UserPermissions.ToList());
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

            if (int.TryParse(paramValue, out caseId))
                return caseId;

            //extract from body input in case of json object
            if (attr.CheckBody)
            {
                try
                {
                    // it is important to use ReadAsStringAsync to keep the request stream position at 0 
                    var content = actionContext.Request.Content.ReadAsStringAsync().Result;
                    
                    var val = (JToken.Parse(content)
                                     .Children<JProperty>()
                                     .FirstOrDefault(x => x.Name.Equals(paramName, StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty).ToString();

                    if (!string.IsNullOrEmpty(val))
                    {
                        if (int.TryParse(val, out caseId))
                            return caseId;
                    }
                }
                //TODO: remove empty catch
                catch (Exception)
                {
                }
            }
            
            return 0;
        }

        protected override bool IgnoreRequest(HttpActionContext actionContext)
        {
            //ignore if action doesn't have attribute
            var hasCasePermissionAttr = CheckAttribute<CheckUserCasePermissionsAttribute>(actionContext, AttributeCheckScope.Action);
            var hasPermissionAttr = CheckAttribute<CheckUserPermissionsAttribute>(actionContext, AttributeCheckScope.Action);
            if (!hasCasePermissionAttr && !hasPermissionAttr)
                return true;
            return false;
        }
    }
}