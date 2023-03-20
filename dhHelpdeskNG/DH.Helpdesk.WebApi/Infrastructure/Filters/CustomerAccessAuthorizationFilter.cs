using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http.Controllers;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using Microsoft.AspNet.Identity;

namespace DH.Helpdesk.WebApi.Infrastructure.Filters
{
    public class CustomerAccessAuthorizationFilter : AuthorizationFilterBase, IAutofacAuthorizationFilter
    {
        private readonly IUserService _userService;

        private const string CustomerIdParamName = "cid";

        #region ctor()

        public CustomerAccessAuthorizationFilter(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region IAutofacAuthorizationFilter Methods

        protected override void AuthorizeRequest(HttpActionContext actionContext)
        {
            var userIdentity = actionContext.ControllerContext.RequestContext.Principal.Identity as ClaimsIdentity;
            var userId = userIdentity.GetUserId().ToInt();
            var customerId = GetCustomerIdFromRequest(actionContext);

            if (!customerId.HasValue)
            {
                actionContext.Response = CreateBadResponse(actionContext.Request, "CustomerId param is missing or empty");
                return;
            }

            if (!ValidateCustomerAccess(customerId.Value, userId))
            {
                actionContext.Response = CreateNotAuthorizedResponse(actionContext.Request);
            }
        }

        protected override bool IgnoreRequest(HttpActionContext actionContext)
        {
            var res = base.IgnoreRequest(actionContext);

            if (res || CheckAttribute<SkipCustomerAuthorization>(actionContext) || CheckAttribute<WebpartSecretKeyHeaderAttribute>(actionContext))
                return true;

            return false;
        }

        #endregion

        #region Private Methods

        private int? GetCustomerIdFromRequest(HttpActionContext actionContext)
        {
            //1. check in route data
            var customerIdVal = actionContext.ControllerContext.RouteData.Values
                .FirstOrDefault(x => x.Key.Equals(CustomerIdParamName, StringComparison.OrdinalIgnoreCase))
                .Value?.ToString();

            //2. look through query string
            if (string.IsNullOrEmpty(customerIdVal))
            {
                var queryParams = actionContext.ControllerContext.Request.GetQueryNameValuePairs();
                customerIdVal = queryParams
                    .FirstOrDefault(x => x.Key.Equals(CustomerIdParamName, StringComparison.OrdinalIgnoreCase))
                    .Value; // no null check requeired since result is a struct        
            }

            //3. Check in action arguemnets
            if (string.IsNullOrEmpty(customerIdVal) && actionContext.ActionArguments.Any())
            {
                customerIdVal = actionContext.ActionArguments
                    .FirstOrDefault(kv => kv.Key.Equals(CustomerIdParamName, StringComparison.OrdinalIgnoreCase))
                    .Value?.ToString();
            }

            //4. Check in request headers
            if (string.IsNullOrEmpty(customerIdVal) && actionContext.Request.Headers.Contains(CustomerIdParamName))
            {
                customerIdVal =
                    actionContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value)
                        .FirstOrDefault(kv => kv.Key.Equals(CustomerIdParamName, StringComparison.OrdinalIgnoreCase))
                        .Value?.ToString();
            }

            return customerIdVal.ToNullableInt();
        }

        private bool ValidateCustomerAccess(int customerId, int userId)
        {
            var userCustomerIds = _userService.GetUserCustomersIds(userId);
            var hasCustomer = userCustomerIds.Any(x => x == customerId);
            return hasCustomer;
        }

        #endregion
    }
}