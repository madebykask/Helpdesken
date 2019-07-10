using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Autofac.Integration.WebApi;
using DH.Helpdesk.Common.Logger;

namespace DH.Helpdesk.WebApi.Infrastructure.Filters
{
    public class ApiExceptionFilter : IAutofacExceptionFilter
    {
        private readonly ILoggerService _logger;

        public ApiExceptionFilter(ILoggerService logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(HttpActionExecutedContext context, CancellationToken cancellationToken)
        {
            //var requestScope = context.Request.GetDependencyScope();
            //var logger = requestScope.GetService(typeof(ILoggerService)) as ILoggerService;
            var errorId = Guid.NewGuid();
            var url = $"{context.ActionContext.RequestContext.Url.Request.RequestUri.AbsoluteUri}";
            var requestId = log4net.LogicalThreadContext.Properties["requestId"]?.ToString();
            //var requestInfo = log4net.LogicalThreadContext.Properties["requestinfo"].ToString();
            var userId = context.ActionContext.RequestContext.Principal.Identity.IsAuthenticated ? context.ActionContext.RequestContext.Principal.Identity.Name : "unauthenticated";
            var customerId = "";//TODO: Where to get selected customer
            _logger.Error(new Logger.ErrorContext(errorId,
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
            return Task.FromResult(0);
        }
    }
}