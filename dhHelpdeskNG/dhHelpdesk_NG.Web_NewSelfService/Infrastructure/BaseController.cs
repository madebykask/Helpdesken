namespace DH.Helpdesk.NewSelfService.Infrastructure
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.NewSelfService.Infrastructure.Extensions;
    using DH.Helpdesk.NewSelfService.Models;
    using DH.Helpdesk.Common.Types;
    using System.Security.Claims;
    using DH.Helpdesk.BusinessData.Models.SSO.Input;
    using System.Configuration;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.NewSelfService.WebServices;

using System.Threading.Tasks;
    using DH.Helpdesk.NewSelfService.WebServices.Common;
    using System.Collections.Generic;
    using DH.Helpdesk.Common.Classes.ServiceAPI.AMAPI.Output;
   


    public class BaseController : Controller
    {
        private readonly IMasterDataService _masterDataService;
        private readonly ISSOService _ssoService;
        private readonly ICaseSolutionService _caseSolutionService;
        

        public BaseController(
            IMasterDataService masterDataService,
            ISSOService ssoService,
            ICaseSolutionService caseSolutionService)
        {
            this._masterDataService = masterDataService;
            this._ssoService = ssoService;
            this._caseSolutionService = caseSolutionService;
        }
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext) //called before a controller action is executed, that is before ~/UserController/index 
        {
            var customerId = -1;

            TempData["ShowLanguageSelect"] = true;

            if (filterContext.ActionParameters.Keys.Contains("customerId"))
            {
                var customerIdPassed = filterContext.ActionParameters["customerId"];
                if (customerIdPassed.ToString() != "")
                    customerId = int.Parse(customerIdPassed.ToString());
            }

            
            if ((SessionFacade.CurrentCustomer != null && SessionFacade.CurrentCustomer.Id != customerId && customerId != -1) || (SessionFacade.CurrentCustomer == null))
            {                
                var newCustomer = this._masterDataService.GetCustomer(customerId);
                SessionFacade.CurrentCustomer = newCustomer;
                ViewBag.PublicCustomerId = customerId;
                ViewBag.PublicCaseTemplate = _caseSolutionService.GetCaseSolutions(customerId).ToList();
            }

            if (SessionFacade.CurrentCustomer == null && customerId == -1)
            {
                filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { message = "Invalid Customer Id! (-1)", errorCode = 101 }));
                SessionFacade.UserHasAccess = false;
            }

            if (filterContext.ActionParameters.Keys.Contains("languageId"))
            {                
                var languageIdPassed = filterContext.ActionParameters["languageId"];
                if (languageIdPassed.ToString() != "")                
                    SessionFacade.CurrentLanguageId = int.Parse(languageIdPassed.ToString());                                    
            }
            else
            {
                if (SessionFacade.CurrentCustomer != null)
                    SessionFacade.CurrentLanguageId = SessionFacade.CurrentCustomer.Language_Id;
            }
                            

            //var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            ClaimsPrincipal principal = User as ClaimsPrincipal;

            if (principal != null)
            {
                string claimData = "";
                bool isFirst = true;
                var userIdentity = new UserIdentity();

                foreach (Claim claim in principal.Claims)
                {
                    var claimTypeArray = claim.Type.Split('/');
                    var pureType = claimTypeArray.LastOrDefault();
                    var value = claim.Value;

                    if (isFirst)
                        claimData = "[" + ((pureType != null) ? pureType.ToString() : "Undefined") + ": " + value.ToString() + "]";
                    else
                        claimData = claimData + " , [" + ((pureType != null) ? pureType.ToString() : "Undefined") + ": " + value.ToString() + "]";

                    isFirst = false;

                    if (pureType != null)
                    {
                        if (pureType.Replace(" ", "").ToLower() == ConfigurationManager.AppSettings["ClaimDomain"].ToString().Replace(" ", "").ToLower())
                            userIdentity.Domain = value;

                        if (pureType.Replace(" ", "").ToLower() == ConfigurationManager.AppSettings["ClaimUserId"].ToString().Replace(" ", "").ToLower())
                            userIdentity.UserId = value;

                        if (pureType.Replace(" ", "").ToLower() == ConfigurationManager.AppSettings["ClaimEmployeeNumber"].ToString().Replace(" ", "").ToLower())
                            userIdentity.EmployeeNumber = value;

                        if (pureType.Replace(" ", "").ToLower() == ConfigurationManager.AppSettings["ClaimFirstName"].ToString().Replace(" ", "").ToLower())
                            userIdentity.FirstName = value;

                        if (pureType.Replace(" ", "").ToLower() == ConfigurationManager.AppSettings["ClaimLastName"].ToString().Replace(" ", "").ToLower())
                            userIdentity.LastName = value;
                        
                        if (pureType.Replace(" ", "").ToLower() == ConfigurationManager.AppSettings["ClaimEmail"].ToString().Replace(" ", "").ToLower())
                            userIdentity.Email = value;
                    }
                }
                
                var netId = principal.Identity.Name;
                var ssoLog = new NewSSOLog()
                {
                    ApplicationId = ConfigurationManager.AppSettings["ApplicationId"].ToString(),
                    NetworkId = netId,
                    ClaimData = claimData,
                    CreatedDate = DateTime.Now
                };

                if (ConfigurationManager.AppSettings["SSOLog"].ToString().ToLower() == "true" && string.IsNullOrEmpty(SessionFacade.CurrentSystemUser))
                    _ssoService.SaveSSOLog(ssoLog);

                //SessionFacade.UserHasAccess = true;

                if (string.IsNullOrEmpty(userIdentity.UserId))
                    SessionFacade.UserHasAccess = false;
                else
                {
                    
                    var defaultEmployeeNumber = ConfigurationManager.AppSettings["DefaultEmployeeNumber"].ToString();
                    if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                        userIdentity.EmployeeNumber = defaultEmployeeNumber;

                    //userIdentity.EmployeeNumber = "31000000";
                    SessionFacade.UserHasAccess = true;
                    if (SessionFacade.CurrentCoWorkers == null)
                    {
                        if (string.IsNullOrEmpty(userIdentity.EmployeeNumber))
                        {
                            SessionFacade.UserHasAccess = false;
                            filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { message = "You don't have access to the portal! (EmployeeNumber is not specified)", errorCode = 101 }));
                        }
                        else
                        {
                            var _amAPIService = new AMAPIService();
                            var employee = AsyncHelpers.RunSync<APIEmployee>(() => _amAPIService.GetEmployeeFor(userIdentity.EmployeeNumber));
                            if (employee.IsManager)
                            {
                                SessionFacade.CurrentCoWorkers = employee.Subordinates;
                                SessionFacade.CurrentSystemUser = userIdentity.UserId;
                                SessionFacade.CurrentUserIdentity = userIdentity;
                            }
                            else
                            {
                                SessionFacade.UserHasAccess = false;
                                SessionFacade.CurrentCoWorkers = new List<SubordinateResponseItem>();
                                filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { message = "You don't have access to the portal! (user is not manager)", errorCode = 102 }));
                            }

                        }
                    }
                }                                     
            }

          
            //this.SessionCheck(filterContext);
            this.SetTextTranslation(filterContext);

        }

        //[HttpPost]
        public ActionResult ChangeLanguage(int languageId)
        {
            SessionFacade.CurrentLanguageId = languageId;
            return this.Json(languageId, JsonRequestBehavior.AllowGet);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext) //called after a controller action is executed, that is after ~/UserController/index 
        {            
            this.SetMasterPageModel(filterContext);
            base.OnActionExecuted(filterContext);
        }

        //protected override void OnAuthorization(AuthorizationContext filterContext)  //called when a process requests authorization or authorization occurs before login and before OnActionExecuting + index + OnActionExecuted 
        //{
        //    var redirectToUrl = "~/login/login?returnUrl=" + filterContext.HttpContext.Request.Url;

        //    //if (SessionFacade.CurrentUser == null)
        //    //{
        //    //    var user = _masterDataService.GetUserForLogin(User.Identity.Name);
        //    //    if (user != null)
        //    //        SessionFacade.CurrentUser = user;
        //    //    else
        //    //        Response.Redirect(redirectToUrl);
        //    //}
        //    //base.OnAuthorization(filterContext);

        //    //if (filterContext.Result == null || (filterContext.Result.GetType() != typeof(HttpUnauthorizedResult)))
        //    //    return;

        //    if (filterContext.HttpContext.Request.IsAjaxRequest())
        //    {
        //        filterContext.Result = filterContext.HttpContext.Request.ContentType == "application/json"
        //            ? (ActionResult)
        //              new JsonResult
        //              {
        //                  Data = new { RedirectTo = redirectToUrl },
        //                  ContentEncoding = System.Text.Encoding.UTF8,
        //                  JsonRequestBehavior = JsonRequestBehavior.DenyGet
        //              }

        //            : new ContentResult
        //            {
        //                Content = redirectToUrl,
        //                ContentEncoding = System.Text.Encoding.UTF8,
        //                ContentType = "text/html"
        //            };

        //        filterContext.HttpContext.Response.StatusCode = 530; //User Access Denied
        //        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        //    }
        //}


        protected string RenderRazorViewToString(string viewName, object model, bool partial = true)
        {
            var viewResult = partial ? ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName) : ViewEngines.Engines.FindView(this.ControllerContext, viewName, null);

            if(viewResult == null || (viewResult != null && viewResult.View == null))
                throw new FileNotFoundException("View could not be found");

            this.ViewData.Model = model;
            using(var sw = new StringWriter())
            {
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(this.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        private void SessionCheck(ActionExecutingContext filterContext)
        {
            if (SessionFacade.CurrentUser != null)
            {
                SessionFacade.CurrentCustomer = SessionFacade.CurrentCustomer ?? this._masterDataService.GetCustomer(SessionFacade.CurrentUser.CustomerId);
                if (SessionFacade.CurrentLanguageId == 0)
                {
                    SessionFacade.CurrentLanguageId = SessionFacade.CurrentUser.LanguageId;
                }
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
                masterViewModel.CustomerSetting = this._masterDataService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);  
            }
            this.ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if (this._masterDataService != null)
            {
                if (SessionFacade.TextTranslation == null)
                    SessionFacade.TextTranslation = this._masterDataService.GetTranslationTexts();

                if (SessionFacade.CurrentUser == null)                                    
                    SessionFacade.CaseTranslation = this._masterDataService.GetCaseTranslations();                
                else                
                  if (SessionFacade.CaseTranslation == null)
                      SessionFacade.CaseTranslation = this._masterDataService.GetCaseTranslations(SessionFacade.CurrentUser.Id);
                
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

            if (this.Roles.ToString() == string.Empty)
                return true;

            foreach (string userRole in this.Roles.ToString().Split(','))
            {
                if (GeneralExtensions.UserHasRole(SessionFacade.CurrentUser, userRole) == true)
                    return true;
            }

            return false;
        }
    }
}