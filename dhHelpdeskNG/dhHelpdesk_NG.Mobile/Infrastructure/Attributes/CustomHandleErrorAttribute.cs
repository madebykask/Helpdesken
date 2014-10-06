namespace DH.Helpdesk.Mobile.Infrastructure.Attributes
{
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Exceptions;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Mobile.Infrastructure.Logger;

    using LogManager = DH.Helpdesk.Mobile.Infrastructure.Logger.LogManager;

    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!this.ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            if (filterContext.Exception is BusinessLogicException)
            {
                return;
            }

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];

            // if the request is AJAX return JSON else view.
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                                           {
                                               JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                                               Data = new
                                                          {
                                                              error = true,
                                                              message = filterContext.Exception.Message
                                                          }
                                           };
            }
            else
            {
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                filterContext.Result = new ViewResult
                                           {
                                               ViewName = this.View,
                                               MasterName = this.Master,
                                               ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                                               TempData = filterContext.Controller.TempData
                                           };
            }

            var workContext = ManualDependencyResolver.Get<IWorkContext>();

            // log the error using log4net.
            LogManager.Error.Error(new ErrorContext(
                                        filterContext.Exception,        
                                        controllerName,
                                        actionName,
                                        filterContext.HttpContext.ApplicationInstance.Context,
                                        workContext).ToString());

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}