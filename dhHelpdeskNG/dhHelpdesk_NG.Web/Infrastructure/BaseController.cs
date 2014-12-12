namespace DH.Helpdesk.Web.Infrastructure
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Models;

    [CustomAuthorize]
    public class BaseController : Controller
    {
        #region Fields

        private readonly IMasterDataService _masterDataService;

        #endregion

        #region Constructors and Destructors

        public BaseController(IMasterDataService masterDataService)
        {
            this._masterDataService = masterDataService;
        }

        #endregion

        #region Methods

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
            //called after a controller action is executed, that is after ~/UserController/index 
        {
            this.SetMasterPageModel(filterContext);
            base.OnActionExecuted(filterContext);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
            //called before a controller action is executed, that is before ~/UserController/index 
        {
            if (SessionFacade.CurrentUser != null)
            {
                this.SessionCheck(filterContext);
                this.SetTextTranslation(filterContext);

                ApplicationFacade.RemoveCaseUserInfo(SessionFacade.CurrentUser.Id);
                ApplicationFacade.UpdateLoggedInUserActivity(this.Session.SessionID);
            }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
            //called when a process requests authorization or authorization occurs before login and before OnActionExecuting + index + OnActionExecuted 
        {
            var redirectToUrl = "~/login/login?returnUrl=" + filterContext.HttpContext.Request.Url;

            if (SessionFacade.CurrentUser == null)
            {
                var user = this._masterDataService.GetUserForLogin(this.User.Identity.Name);
                if (user != null)
                {
                    /// here we have user session expired before auth session expiration
                    SessionFacade.CurrentUser = user;
                    var customerName = this._masterDataService.GetCustomer(user.CustomerId).Name;
                    ApplicationFacade.AddLoggedInUser(
                        new LoggedInUsers
                        {
                            Customer_Id = user.CustomerId,
                            User_Id = user.Id,
                            UserFirstName = user.FirstName,
                            UserLastName = user.SurName,
                            CustomerName = customerName,
                            LoggedOnLastTime = DateTime.UtcNow,
                            SessionId = this.Session.SessionID
                        });
                } 
                else
                {
                    this.Response.Redirect(redirectToUrl);
                }
            }

            base.OnAuthorization(filterContext);

            if (filterContext.Result == null || (filterContext.Result.GetType() != typeof(HttpUnauthorizedResult)))
            {
                return;
            }

            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{

            //    filterContext.Result = filterContext.HttpContext.Request.ContentType == "application/json"
            //        ? (ActionResult)
            //          new JsonResult
            //          {
            //              Data = new { RedirectTo = redirectToUrl },
            //              ContentEncoding = System.Text.Encoding.UTF8,
            //              JsonRequestBehavior = JsonRequestBehavior.DenyGet
            //          }

            //        : new ContentResult
            //        {
            //            Content = redirectToUrl,
            //            ContentEncoding = System.Text.Encoding.UTF8,
            //            ContentType = "text/html"
            //        };

            //    filterContext.HttpContext.Response.StatusCode = 530; //User Access Denied
            //    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
            //}
        }

        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial
                ? ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName)
                : ViewEngines.Engines.FindView(this.ControllerContext, viewName, null);

            if (viewResult == null || (viewResult != null && viewResult.View == null))
            {
                throw new FileNotFoundException("View could not be found");
            }

            this.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext(
                    this.ControllerContext,
                    viewResult.View,
                    this.ViewData,
                    this.TempData,
                    sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(this.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private void SessionCheck(ActionExecutingContext filterContext)
        {
            if (SessionFacade.CurrentUser != null)
            {
                SessionFacade.CurrentCustomer = SessionFacade.CurrentCustomer
                                                ?? this._masterDataService.GetCustomer(
                                                    SessionFacade.CurrentUser.CustomerId);
            }
        }

        private void SetMasterPageModel(ActionExecutedContext filterContext)
        {
            var masterViewModel = new MasterPageViewModel();
            masterViewModel.Languages = this._masterDataService.GetLanguages();
            masterViewModel.SelectedLanguageId = SessionFacade.CurrentLanguageId;
            if (SessionFacade.CurrentUser != null)
            {
                masterViewModel.Customers = this._masterDataService.GetCustomers(SessionFacade.CurrentUser.Id);
            }
            if (SessionFacade.CurrentCustomer != null)
            {
                masterViewModel.SelectedCustomerId = SessionFacade.CurrentCustomer.Id;
                masterViewModel.CustomerSetting =
                    this._masterDataService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            }
            this.ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if (this._masterDataService != null)
            {
                if (SessionFacade.TextTranslation == null)
                {
                    SessionFacade.TextTranslation = this._masterDataService.GetTranslationTexts();
                }
                if (SessionFacade.CaseTranslation == null && SessionFacade.CurrentUser != null)
                {
                    SessionFacade.CaseTranslation =
                        this._masterDataService.GetCaseTranslations(SessionFacade.CurrentUser.Id);
                }
            }
        }

        #endregion
    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        #region Methods

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            if (httpContext.Session == null)
            {
                httpContext.Response.Redirect("~/login/login");
                return false;
            }

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            if (this.Roles == string.Empty)
            {
                return true;
            }

            foreach (string userRole in this.Roles.Split(','))
            {
                if (GeneralExtensions.UserHasRole(SessionFacade.CurrentUser, userRole))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}