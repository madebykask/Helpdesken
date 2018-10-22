using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using DH.Helpdesk.WebApi.Infrastructure.Extensions;

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
                         ErrorMessage = modelState.BuildModelStateErrorSummary()
#else
                         ErrorMessage = modelState.BuildModelStateErrorSummary(false)
#endif
                     });
        }
    }
}