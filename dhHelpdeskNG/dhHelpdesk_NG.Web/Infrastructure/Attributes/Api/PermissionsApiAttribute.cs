using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Infrastructure.Attributes.Api
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class PermissionsApiAttribute : WebApiAuthorizeAttribute
	{
		private string _userPermission;

		public string UserPermsissions
		{
			get
			{
				return _userPermission ?? string.Empty;
			}
			set
			{
				_userPermission = value;
			}
		}

		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			if (!actionContext.RequestContext.Principal.Identity.IsAuthenticated)
			{
				return false;
			}

			if (Roles != string.Empty)
			{
				return Roles.Split(',').Any(userRole => GeneralExtensions.UserHasRole(SessionFacade.CurrentUser, userRole));
			}

			if (UserPermsissions != string.Empty)
			{
				return UserPermsissions.Split(',').Any(userPermission => GeneralExtensions.UserHasPermission(SessionFacade.CurrentUser, userPermission));
			}

			// NO any specific ACL politic is set
			if (Roles == string.Empty && UserPermsissions == string.Empty)
			{
				return true;
			}


			return false;
		}

		protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
		{
			if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
			{
				var response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
				actionContext.Response = response;
				return;
			}

			base.HandleUnauthorizedRequest(actionContext);
		}
	}
}