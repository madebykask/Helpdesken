using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Services.Infrastructure;
using DH.Helpdesk.Web.Infrastructure.Logger;

namespace DH.Helpdesk.Web.Infrastructure.Attributes.Api
{
    public class CustomApiErrorFilter : ExceptionFilterAttribute
	{
		public CustomApiErrorFilter()
		{
		}

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			var logger = LogManager.Error;
			var errorId = Guid.NewGuid();
			var workContext = ManualDependencyResolver.Get<IWorkContext>();
			var httpContext = GetHttpContext(actionExecutedContext);
			logger.Error(new ErrorContext(errorId,
										actionExecutedContext.Exception,
										actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,
										actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
										httpContext,
										workContext).ToString());

            Services.Services.DataLogService.SaveLog(new ErrorContext(errorId,
                                        actionExecutedContext.Exception,
                                        actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName,
                                        actionExecutedContext.ActionContext.ActionDescriptor.ActionName,
                                        httpContext,
                                        workContext).ToString(), DH.Helpdesk.Common.Enums.DataLogTypes.GENERAL);

#if DEBUG
            actionExecutedContext.Response = actionExecutedContext.Request
					.CreateResponse(HttpStatusCode.InternalServerError,
					actionExecutedContext.Exception);
#else
			actionExecutedContext.Response = actionExecutedContext.Request
					.CreateResponse(HttpStatusCode.InternalServerError,
					String.Format("Sorry, an error occurred while processing your request.\r\nPlease provide below to your support team:\r\nError Id\r\nStep-by-step instructions on how to reproduce your issue\r\nTime when error occurred\r\n{0}", errorId));
#endif
		}

		/// <summary>
		/// Returns HttpContext from HttpActionExecutedContext. Works only in IIS.
		/// </summary>
		/// <param name="actionExecutedContext"></param>
		/// <returns></returns>
		private HttpContext GetHttpContext(HttpActionExecutedContext actionExecutedContext)
		{
			var request = actionExecutedContext.Request;
			object value;
			if (request == null
				|| !request.Properties.TryGetValue("MS_HttpContext", out value)
				|| !(value is HttpContextBase))
				return null; 

			var httpContextBase = (HttpContextBase) value;
			return httpContextBase.ApplicationInstance.Context;
		}
	}
}