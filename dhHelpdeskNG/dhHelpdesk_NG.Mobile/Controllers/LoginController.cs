namespace DH.Helpdesk.Mobile.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Infrastructure;

    public class LoginController : Controller
    {
        private readonly IUserService userService;
        private readonly ICustomerService customerService;

        public LoginController(IUserService userService, ICustomerService customerService)
        {
            this.userService = userService;
            this.customerService = customerService;
        }

        public ActionResult Login()
        {
            this.Session.Clear();
            ApplicationFacade.RemoveLoggedInUser(Session.SessionID);
            FormsAuthentication.SignOut();
            return this.View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection coll)
        {
            string userName = coll["txtUid"].ToString().Trim();
            string password = coll["txtPwd"].ToString().Trim();
            string returnUrl = Request.QueryString["returnUrl"];
            string decodedUrl = "/";

            if (!this.IsValidLoginArgument(userName, password))
            {
                return this.View("Login");
            }

            var user = this.userService.Login(userName, password);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    decodedUrl = this.Server.UrlDecode(returnUrl);
                }

                if (!this.Url.IsLocalUrl(decodedUrl))
                {
                    decodedUrl = "/";
                }

                if (decodedUrl != null && decodedUrl.Contains("login"))
                {
                    decodedUrl = "/";
                }

                SessionFacade.CurrentUser = user;
                SessionFacade.CurrentLanguageId = user.LanguageId;
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
                            SessionId = this.Session.SessionID
                        });

                this.RedirectFromLoginPage(userName, decodedUrl);
            }
            else
            {
                this.TempData["LoginFailed"] = "Login failed! The user name or password entered is incorrect.";
            }

            return this.View("Login");
        }

        [NonAction]
        private bool IsValidLoginArgument(string userName, string password)
        {
            return !(string.IsNullOrEmpty(userName) && string.IsNullOrEmpty(password));
        }

        [NonAction]
        private void RedirectFromLoginPage(string userName, string returnURL)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddDays(10), false, SessionFacade.CurrentUser.ToString());

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            this.Response.Cookies.Add(cookie);

            if(!string.IsNullOrEmpty(returnURL))
                this.Response.Redirect(returnURL);
            else
                this.Response.Redirect(FormsAuthentication.DefaultUrl);
        }
    }
}
