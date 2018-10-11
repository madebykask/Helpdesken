using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Filters
{
	/// <summary>
	/// Filter checks if model is valid.
	/// Aborts request if invalid with description in responce
	/// </summary>
	public class ModelValidationApiActionFilter : ActionFilterAttribute
	{

		public ModelValidationApiActionFilter()
		{
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			var modelState = actionContext.ModelState;

			if (!modelState.IsValid)
				actionContext.Response =
					actionContext.Request
					 .CreateResponse(HttpStatusCode.BadRequest, new
					 {
#if DEBUG
						 ErrorMessage = "Invalid model: " + string.Join("; ", modelState.Where(x => x.Value.Errors.Any()).Select(x =>
						     $"{x.Key} : {string.Join(",", x.Value.Errors.Select(e => $"Error: {e.ErrorMessage ?? ""}, Exception: {e.Exception?.ToString() ?? ""}"))}"))
#else
					 	 ErrorMessage = string.Join("; ", modelState.Where(x => x.Value.Errors.Any()).Select(x => string.Join(",", x.Value.Errors.Select(e => e.ErrorMessage ?? ""))))
#endif
					 });
		}
	}
}