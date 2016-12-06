using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Routing;

namespace DH.Helpdesk.Web.Infrastructure.Attributes.Api
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class WebApiAuthorizeAttribute : AuthorizeAttribute
	{
		public WebApiAuthorizeAttribute()
		{
		}

		protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
		{
			var urlHelper = new UrlHelper(actionContext.Request);
			var redirectUrl = actionContext.Request.Headers.Referrer != null
				? actionContext.Request.Headers.Referrer.OriginalString
				: urlHelper.Route("Default", new { controller = "Home", action = "Index" });
			var response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { RedirectTo = redirectUrl });
			actionContext.Response = response;

			//base.HandleUnauthorizedRequest(actionContext);
		}
	}
}