namespace DH.Helpdesk.Mobile.Infrastructure.ActionFilters
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Mobile.Infrastructure.Extensions;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class BadRequestOnNotValidAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var modelErrors = modelState.GetErrors();
                var message = new StringBuilder();
                foreach (var modelError in modelErrors)
                {
                    message.AppendLine(string.Format(
                                @"Field: ""{0}""; Value: ""{1}""; Errors: ""{2}""",
                                modelError.Field,
                                modelError.Value,
                                string.Join("; ", modelError.Errors)));
                }

                throw new HttpException((int)HttpStatusCode.BadRequest, message.ToString());
            }

            base.OnActionExecuting(filterContext);
        }
    }
}