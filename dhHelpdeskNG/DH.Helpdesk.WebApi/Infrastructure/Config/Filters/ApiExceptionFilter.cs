using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Services.Services;
using Microsoft.Owin;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var logger = ManualDependencyResolver.Get<ILoggerService>(Log4NetLoggerService.LogType.ERROR);//TODO: Get rid of ManualDependencyResolver, replace it with public field injection
            var errorId = Guid.NewGuid();
            var url = $"{context.ActionContext.RequestContext.Url.Request.RequestUri.AbsoluteUri}";
            var requestId = log4net.LogicalThreadContext.Properties["requestId"].ToString();
            //var requestInfo = log4net.LogicalThreadContext.Properties["requestinfo"].ToString();
            var userId = context.ActionContext.RequestContext.Principal.Identity.IsAuthenticated ? context.ActionContext.RequestContext.Principal.Identity.Name : "unauthenticated";
            var customerId = "";//TODO: Where to get selected customer
            logger.Error(new Logger.ErrorContext(errorId,
                                        context.Exception,
                                        context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,
                                        context.ActionContext.ActionDescriptor.ActionName,
                                        url,
                                        userId,
                                        customerId,
                                        requestId
                                        ).ToString());

#if DEBUG
            context.Response = context.Request
                    .CreateResponse(HttpStatusCode.InternalServerError,
                    context.Exception);
#else
			context.Response = context.Request
					.CreateResponse(HttpStatusCode.InternalServerError,
					String.Format("Sorry, an error occurred while processing your request.\r\nPlease provide Error Id to support team:\r\n{0}", errorId));
#endif
        }
    }
}