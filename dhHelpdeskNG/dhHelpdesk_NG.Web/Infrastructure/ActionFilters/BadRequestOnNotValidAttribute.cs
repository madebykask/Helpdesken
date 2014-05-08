namespace DH.Helpdesk.Web.Infrastructure.ActionFilters
{
    using System;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class BadRequestOnNotValidAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}