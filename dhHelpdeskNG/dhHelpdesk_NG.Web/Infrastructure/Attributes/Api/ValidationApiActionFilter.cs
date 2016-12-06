using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DH.Helpdesk.Web.Infrastructure.Attributes.Api
{
	/// <summary>
	/// Filter checks if model is valid.
	/// Aborts request if invalid with description in responce
	/// </summary>
	public class ValidationApiActionFilter : ActionFilterAttribute
	{

		public ValidationApiActionFilter()
		{
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var modelState = actionContext.ModelState;

			if (!modelState.IsValid)
				actionContext.Response =
					actionContext.Request
					 .CreateResponse(HttpStatusCode.OK, new
					 {
#if DEBUG
						 ErrorMessage = String.Join("; ", modelState.Where(x => x.Value.Errors.Any()).Select(x => String.Format("{0} : {1}", x.Key, String.Join(",", x.Value.Errors.Select(e => e.ErrorMessage)))))
#else
					 	 ErrorMessage = String.Join("; ", modelState.Where(x => x.Value.Errors.Any()).Select(x => String.Join(",", x.Value.Errors.Select(e => e.ErrorMessage))))
#endif
					 });
		}
	}
}