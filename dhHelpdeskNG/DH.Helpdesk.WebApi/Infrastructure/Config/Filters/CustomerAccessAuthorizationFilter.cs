using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure.Attributes;
using Microsoft.AspNet.Identity;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Filters
{
    //todo: https://stackoverflow.com/questions/12817202/accessing-post-or-get-parameters-in-custom-authorization-mvc4-web-api
    public class CustomerAccessAuthorizationFilter : IAutofacAuthorizationFilter
    {
        private readonly IUserService _userService;

        private const string CustomerIdParamName = "cid";

        #region ctor()

        public CustomerAccessAuthorizationFilter(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        public Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var userIdentity = actionContext.ControllerContext.RequestContext.Principal.Identity as ClaimsIdentity;
            if (CheckIgnoreAttributes(actionContext))
                return Task.FromResult(0);

            if (userIdentity != null && userIdentity.IsAuthenticated)
            {
                var userId = userIdentity.GetUserId().ToInt();
                var customerId = GetCustomerIdFromRequest(actionContext);

                if (!customerId.HasValue)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "CustomerId param is missing or empty");
                    return Task.FromResult(0);
                }

                if (!ValidateCustomerAccess(customerId.Value, userId))
                {
                    HandleUserIsNotAuthorised(actionContext);
                }
            }
            else
            {
                HandleUnAuthenticated(actionContext);
            }

            return Task.FromResult(0);
        }

        private bool CheckIgnoreAttributes(HttpActionContext actionContext)
        {
            var actionDescriptor = actionContext.ActionDescriptor;

            var hasAttr = actionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(false).Any() ||
                          actionDescriptor.GetCustomAttributes<SkipCustomerAuthorization>(false).Any();

            if (hasAttr)
                return true;

            var controllerDescriptor = actionContext.ControllerContext.ControllerDescriptor;
                
            hasAttr = controllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>(false).Any() ||
                        controllerDescriptor.GetCustomAttributes<SkipCustomerAuthorization>(false).Any();

            return hasAttr;
        }

        private void HandleUserIsNotAuthorised(HttpActionContext actionContext)
        {
            actionContext.Response =
                actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User is not authorized");
        }

        private void HandleUnAuthenticated(HttpActionContext actionContext)
        {
            actionContext.Response =
                actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User is not authenticated");
        }

        private static int? GetCustomerIdFromRequest(HttpActionContext actionContext)
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
    }
}