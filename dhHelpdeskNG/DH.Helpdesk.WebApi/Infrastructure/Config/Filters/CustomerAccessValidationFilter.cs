using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.Filters
{
    public class CustomerAccessValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //TODO: if action parameter CustomerId does not belong to users customer - error.
        }
    }
}