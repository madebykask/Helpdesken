using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure.Extensions;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Infrastructure
{
    [CustomAuthorize]
    public class BaseController : Controller
    {
        private readonly IMasterDataService _masterDataService;

        public BaseController(
            IMasterDataService masterDataService)
        {
            _masterDataService = masterDataService;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) //called before a controller action is executed, that is before ~/UserController/index 
        {
            if (SessionFacade.CurrentUser == null)
            {
                string strLogin = User.Identity.Name;
                var u = _masterDataService.GetUserForLogin(strLogin);

                if (u != null)
                {
                    SessionFacade.CurrentUser = u;
                }
                else
                {
                    Response.Redirect("/Login/Login");
                }
            }

            SessionCheck(filterContext);
            SetTextTranslation(filterContext);  
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext) //called after a controller action is executed, that is after ~/UserController/index 
        {
            SetMasterPageModel(filterContext);
            base.OnActionExecuted(filterContext);
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)  //called when a process requests authorization or authorization occurs before login and before OnActionExecuting + index + OnActionExecuted 
        {
            base.OnAuthorization(filterContext);

            if (filterContext.Result == null || (filterContext.Result.GetType() != typeof(HttpUnauthorizedResult)))
                return;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var redirectToUrl = "/login?returnUrl=" + filterContext.HttpContext.Request.UrlReferrer.PathAndQuery;

                filterContext.Result = filterContext.HttpContext.Request.ContentType == "application/json"
                    ? (ActionResult)
                      new JsonResult
                      {
                          Data = new { RedirectTo = redirectToUrl },
                          ContentEncoding = System.Text.Encoding.UTF8,
                          JsonRequestBehavior = JsonRequestBehavior.DenyGet
                      }

                    : new ContentResult
                    {
                        Content = redirectToUrl,
                        ContentEncoding = System.Text.Encoding.UTF8,
                        ContentType = "text/html"
                    };

                filterContext.HttpContext.Response.StatusCode = 530; //User Access Denied
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            }

            if (filterContext.Result is HttpUnauthorizedResult)
                filterContext.Result = new RedirectResult("/");
        }

        //protected string RenderRazorViewToString(string viewName, object model)
        //{
        //    ViewData.Model = model;
        //    using (var sw = new StringWriter())
        //    {
        //        var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
        //        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}

        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial ? ViewEngines.Engines.FindPartialView(ControllerContext, viewName) : ViewEngines.Engines.FindView(ControllerContext, viewName, null);

            if(viewResult == null || (viewResult != null && viewResult.View == null))
                throw new FileNotFoundException("View could not be found");

            ViewData.Model = model;
            using(var sw = new StringWriter())
            {
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private void SessionCheck(ActionExecutingContext filterContext)
        {
            //SessionFacade.CurrentCustomer = SessionFacade.CurrentCustomer ?? new Customer { Id = SessionFacade.CurrentUser.Customer_Id };
            SessionFacade.CurrentCustomer = SessionFacade.CurrentCustomer ?? _masterDataService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
            if (SessionFacade.CurrentLanguage == 0)
            {
                SessionFacade.CurrentLanguage = SessionFacade.CurrentUser.LanguageId;
            }
        }

        private void SetMasterPageModel(ActionExecutedContext filterContext)
        {
            var masterViewModel = new MasterPageViewModel();
            masterViewModel.Languages = _masterDataService.GetLanguages();
            masterViewModel.SelectedLanguageId = SessionFacade.CurrentLanguage;
            masterViewModel.Customers = _masterDataService.GetCustomers(SessionFacade.CurrentUser.Id); 
            masterViewModel.SelectedCustomerId = SessionFacade.CurrentCustomer.Id;
            ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if (_masterDataService != null)
            {
                if (SessionFacade.TextTranslation == null)
                    SessionFacade.TextTranslation = _masterDataService.GetTranslationTexts();
                if (SessionFacade.CaseTranslation == null && SessionFacade.CurrentUser != null)
                    SessionFacade.CaseTranslation = _masterDataService.GetCaseTranslations(SessionFacade.CurrentUser.Id); 
            }
        }

    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            if (!httpContext.User.Identity.IsAuthenticated)
                return false;

            if (Roles.ToString() == string.Empty)
                return true;

            foreach (string userRole in Roles.ToString().Split(','))
            {
                if (GeneralExtensions.UserHasRole(SessionFacade.CurrentUser, userRole) == true)
                    return true;
            }

            return false;
        }
    }
}