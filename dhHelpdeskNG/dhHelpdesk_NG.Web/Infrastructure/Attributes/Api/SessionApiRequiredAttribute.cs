using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace DH.Helpdesk.Web.Infrastructure.Attributes.Api
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class SessionApiRequiredAttribute : WebApiAuthorizeAttribute
	{
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			return base.IsAuthorized(actionContext) && HttpContext.Current != null && HttpContext.Current.Session != null;
		}
	}
}