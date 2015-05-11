namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using DH.Helpdesk.BusinessData.Enums.Users;
    using DH.Helpdesk.Services.Infrastructure.TimeZoneResolver;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Users;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;
    using DH.Helpdesk.Common.Enums;

    public class LoginController : Controller
    {
        private const string Root = "/";
        private readonly IUserService userService;
        private readonly ICustomerService customerService;
        private readonly ILanguageService languageService;
        private readonly IUsersPasswordHistoryService usersPasswordHistoryService;

        private readonly IRouteResolver routeResolver;

        public LoginController(
                IUserService userService, 
                ICustomerService customerService, 
                IUsersPasswordHistoryService usersPasswordHistoryService,
                ILanguageService languageService, 
                IRouteResolver routeResolver)
        {
            this.userService = userService;
            this.customerService = customerService;
            this.usersPasswordHistoryService = usersPasswordHistoryService;
            this.languageService = languageService;
            this.routeResolver = routeResolver;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (this.Session != null)
            {
                this.Session.Clear();
                ApplicationFacade.RemoveLoggedInUser(this.Session.SessionID);  
                this.Session.Abandon();
            }

            FormsAuthentication.SignOut();
            return this.View("Login");
        }

        [HttpGet]
        public ViewResult Login()
        {            
            return this.View();
        } 

        [HttpPost]
        public ActionResult Login(FormCollection coll, string returnUrl)
        {
            string userName = coll["txtUid"].Trim();
            string password = coll["txtPwd"].Trim();

            if (this.IsValidLoginArgument(userName, password))
            {
                var user = this.userService.Login(userName, password);

                if (user != null)
                {
                    var redirectTo = string.Empty;
                    if (!string.IsNullOrEmpty(returnUrl) 
                        && Url.IsLocalUrl(returnUrl)
                        && this.routeResolver.AbsolutePathToRelative(returnUrl) != Root)
                    {
                        redirectTo = Server.UrlDecode(returnUrl);
                    }

                    if (!string.IsNullOrEmpty(redirectTo) 
                        && redirectTo.ToLower().Contains("login"))
                    {
                        redirectTo = Root;
                    }

                    this.Session.Clear();
                    int timeZoneOffsetInJan1, timeZoneOffsetInJul1;
                    if (int.TryParse(coll["timeZoneOffsetInJan1"], out timeZoneOffsetInJan1)
                        && int.TryParse(coll["timeZoneOffsetInJul1"], out timeZoneOffsetInJul1))
                    {
                        TimeZoneInfo[] tzones;
                        SessionFacade.TimeZoneDetectionResult = TimeZoneResolver.DetectTimeZone(timeZoneOffsetInJan1, timeZoneOffsetInJul1, out tzones);
                        if (string.IsNullOrEmpty(user.TimeZoneId))
                        {
                            user.TimeZoneId = SessionFacade.TimeZoneDetectionResult == TimeZoneAutodetectResult.Failure ? TimeZoneInfo.Local.Id : tzones[0].Id;
                        }
                        else
                        {
                            if (tzones.All(it => it.Id != user.TimeZoneId))
                            {
                                /// notice to user about to change his time zone in profile
                                SessionFacade.TimeZoneDetectionResult = TimeZoneAutodetectResult.Notice;
                            }
                            else
                            {
                                /// no changes in users time zone was made
                                SessionFacade.TimeZoneDetectionResult = TimeZoneAutodetectResult.None;
                            }
                        }
                    }
                    else
                    {
                       user.TimeZoneId = TimeZoneInfo.Local.Id;
                       SessionFacade.TimeZoneDetectionResult = TimeZoneAutodetectResult.Failure;
                    }

                    SessionFacade.CurrentUser = user;
                    SessionFacade.CurrentLanguageId = user.LanguageId;
                    SessionFacade.CurrentLoginMode = LoginMode.Application;

                    var language = this.languageService.GetLanguage(user.LanguageId);

                    if (language != null) 
                    {
                        SessionFacade.CurrentLanguageCode = language.LanguageID;
                    }

                    var customer = this.customerService.GetCustomer(user.CustomerId);
                    ApplicationFacade.AddLoggedInUser(
                        new LoggedInUsers
                            {
                                Customer_Id = user.CustomerId,
                                User_Id = user.Id,
                                UserFirstName = user.FirstName,
                                UserLastName = user.SurName,
                                CustomerName = customer.Name,
                                LoggedOnLastTime = DateTime.UtcNow,
                                SessionId = Session.SessionID
                            });

                    this.usersPasswordHistoryService.SaveHistory(user.Id, EncryptionHelper.GetMd5Hash(password));
                    if (SessionFacade.TimeZoneDetectionResult == TimeZoneAutodetectResult.Failure
                        || SessionFacade.TimeZoneDetectionResult == TimeZoneAutodetectResult.MoreThanOne)
                    {
                        this.RedirectFromLoginPage(userName, "~/Profile/Edit/", user.StartPage);
                        return null;
                    }
                    
                    this.RedirectFromLoginPage(userName, redirectTo, user.StartPage);
                }
                else
                {
                    this.TempData["LoginFailed"] = "Login failed! The user name or password entered is incorrect.";
                }
            }

            return this.View("Login");
        }

        [NonAction]
        private bool IsValidLoginArgument(string userName, string password)
        {
            return !(string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password));
        }

        [NonAction]
        private void RedirectFromLoginPage(string userName, string returnUrl, int startPage)
        {
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddDays(10), false, SessionFacade.CurrentUser.ToString());

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            this.Response.Cookies.Add(cookie);

            this.Response.Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : this.routeResolver.ResolveStartPage(this.Url, startPage));
        }
    }
}
