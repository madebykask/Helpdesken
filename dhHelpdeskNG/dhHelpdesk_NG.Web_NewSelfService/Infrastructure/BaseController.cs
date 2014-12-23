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
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    

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

            if(filterContext.ActionParameters.Keys.Contains("customerId"))
            {
                var customerIdPassed = filterContext.ActionParameters["customerId"];
                if(customerIdPassed.ToString() != "")
                    customerId = int.Parse(customerIdPassed.ToString());
            }

            if((SessionFacade.CurrentCustomer != null && SessionFacade.CurrentCustomer.Id != customerId && customerId != -1) || (SessionFacade.CurrentCustomer == null))
            {
                var newCustomer = this._masterDataService.GetCustomer(customerId);
                SessionFacade.CurrentCustomer = newCustomer;
                ViewBag.PublicCustomerId = customerId;
                ViewBag.PublicCaseTemplate = _caseSolutionService.GetCaseSolutions(customerId).ToList();

                // Customer changed clear sessions
                SessionFacade.CurrentCoWorkers = null;
            }

            if (SessionFacade.AllLanguages == null)
            {
                SessionFacade.AllLanguages = GetActiveLanguages();
            }

            if(SessionFacade.CurrentCustomer == null)
            {
                SessionFacade.UserHasAccess = false;
                filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { message = string.Format("Invalid Customer Id! ({0})", customerId), errorCode = 101 }));
            }

            if(filterContext.ActionParameters.Keys.Contains("languageId"))
            {
                var languageIdPassed = filterContext.ActionParameters["languageId"];
                if(languageIdPassed.ToString() != "")
                    SessionFacade.CurrentLanguageId = int.Parse(languageIdPassed.ToString());
            }
            else
            {
                if(SessionFacade.CurrentCustomer != null & (SessionFacade.CurrentLanguageId == null || (SessionFacade.CurrentLanguageId != null && SessionFacade.CurrentLanguageId == 0)))
                    SessionFacade.CurrentLanguageId = SessionFacade.CurrentCustomer.Language_Id;
            }
           
            if (ConfigurationManager.AppSettings["LoginMode"].ToString() == "SSO")
            {
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

                    if (string.IsNullOrEmpty(userIdentity.UserId))
                    {
                        SessionFacade.UserHasAccess = false;
                        filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { message = "You don't have access to the portal! (User Id is not specified)", errorCode = 101 }));
                    }
                    else
                    {
                        SessionFacade.CurrentSystemUser = userIdentity.UserId;
                        SessionFacade.CurrentUserIdentity = userIdentity;

                        var defaultEmployeeNumber = ConfigurationManager.AppSettings["DefaultEmployeeNumber"].ToString();
                        if (!string.IsNullOrEmpty(defaultEmployeeNumber))
                            userIdentity.EmployeeNumber = defaultEmployeeNumber;

                        SessionFacade.UserHasAccess = true;

                        if (SessionFacade.CurrentCustomer != null && !string.IsNullOrEmpty(userIdentity.EmployeeNumber))
                        {
                            var config = (ECT.FormLib.Configurable.AccessManagment)System.Configuration.ConfigurationManager.GetSection("formLibConfigurable/accessManagment");
                            var country = config.Countries.Where(x => x.HelpdeskCustomerId == SessionFacade.CurrentCustomer.Id.ToString()).FirstOrDefault();

                            if (country != null && !userIdentity.EmployeeNumber.StartsWith(country.EmployeePrefix))
                            {
                                SessionFacade.UserHasAccess = false;
                                SessionFacade.CurrentCoWorkers = new List<SubordinateResponseItem>();
                                filterContext.Result = new RedirectResult(Url.Action("Index", "Error", new { message = "You don't have access to the portal! (user is not manager for country)", errorCode = 103 }));
                            }
                        }

                        if (SessionFacade.CurrentCoWorkers == null || (SessionFacade.CurrentCoWorkers != null && SessionFacade.CurrentCoWorkers.Count == 0))
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
            } // SSO Login
            else
            if (ConfigurationManager.AppSettings["LoginMode"].ToString() == "Windows")
            {
                var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                SessionFacade.UserHasAccess = true;
                SessionFacade.CurrentCoWorkers = null;                                
                string fullName = identity.Name;
                string userId = fullName.GetUserFromAdPath();
                string userDomain = fullName.GetDomainFromAdPath();

                SessionFacade.CurrentSystemUser = userId;
                var ui = new UserIdentity()
                {
                    UserId = userId,
                    Domain = userDomain,
                    FirstName = userId
                };

                SessionFacade.CurrentUserIdentity = ui;
            }


            this.SetTextTranslation(filterContext);
        }
                
        public ActionResult ChangeLanguage(string language, string currentUrl, string lastParams)
        {
            if (SessionFacade.AllLanguages != null)
            {
                List<LanguageOverview> allLang = SessionFacade.AllLanguages.ToList();
                var lId = allLang.Where(a => a.LanguageId == language).Select(a => a.Id).SingleOrDefault();
                SessionFacade.CurrentLanguageId = lId;
            }

            
            
            currentUrl += "?language=" + language;

            var allParam = lastParams.Split('&');
            foreach (var param in allParam)
            {
                if ((!string.IsNullOrEmpty(param)) &&  (!param.ToLower().Contains("language=")) )
                    currentUrl += "&" + param;
            }

            return Redirect(currentUrl);            
        }

        private List<LanguageOverview> GetActiveLanguages()
        {
            var activeLangs = _masterDataService.GetLanguages()
                                        .Where(l => l.IsActive == 1)
                                        .Select(la => new LanguageOverview { Id = la.Id, IsActive = la.IsActive.convertIntToBool(), LanguageId = la.LanguageID, Name = la.Name })
                                        .OrderBy(l => l.Name)
                                        .ToList();
            return activeLangs;
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
            }
        }

        private void SetMasterPageModel(ActionExecutedContext filterContext)
        {
            var masterViewModel = new MasterPageViewModel();
            masterViewModel.Languages = this._masterDataService.GetLanguages();
            masterViewModel.SelectedLanguageId = SessionFacade.CurrentLanguageId;
            if(SessionFacade.CurrentUser != null)
            {
                masterViewModel.Customers = this._masterDataService.GetCustomers(SessionFacade.CurrentUser.Id);
            }
            if(SessionFacade.CurrentCustomer != null)
            {
                masterViewModel.SelectedCustomerId = SessionFacade.CurrentCustomer.Id;
                masterViewModel.CustomerSetting = this._masterDataService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            }
            this.ViewData[Constants.ViewData.MasterViewData] = masterViewModel;
        }

        private void SetTextTranslation(ActionExecutingContext filterContext)
        {
            if(this._masterDataService != null)
            {
                // TextTypeId 300  is used for Line Manager
                if(SessionFacade.TextTranslation == null)
                    SessionFacade.TextTranslation = this._masterDataService.GetTranslationTexts(); //.Where(x=> x.Type == 300).ToList();

                if(SessionFacade.CurrentUser == null)
                    SessionFacade.CaseTranslation = this._masterDataService.GetCaseTranslations();
                else
                    if(SessionFacade.CaseTranslation == null)
                        SessionFacade.CaseTranslation = this._masterDataService.GetCaseTranslations(SessionFacade.CurrentUser.Id);

            }
        }
    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(httpContext == null)
                throw new ArgumentNullException("httpContext");

            if(!httpContext.User.Identity.IsAuthenticated)
                return false;

            if(this.Roles.ToString() == string.Empty)
                return true;

            foreach(string userRole in this.Roles.ToString().Split(','))
            {
                if(GeneralExtensions.UserHasRole(SessionFacade.CurrentUser, userRole) == true)
                    return true;
            }

            return false;
        }
    }
}