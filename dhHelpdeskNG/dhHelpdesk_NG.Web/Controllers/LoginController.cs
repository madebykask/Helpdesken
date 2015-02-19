namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Users;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Tools;

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
            this.Session.Clear();
            ApplicationFacade.RemoveLoggedInUser(Session.SessionID);
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
                    SessionFacade.CurrentUser = user;
                    SessionFacade.CurrentLanguageId = user.LanguageId;

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
