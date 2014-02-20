namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class LoginController : Controller
    {
        private readonly IUserService userService;

        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Login(string returnUrl)
        {
            this.Session.Clear();
            FormsAuthentication.SignOut();

            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);
            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
                ViewBag.ReturnURL = returnUrl;

            return this.View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection coll)
        {
            string userName = coll["txtUid"].ToString().Trim();
            string password = coll["txtPwd"].ToString().Trim();
            string returnUrl = coll["returnUrl"].ToString().Trim();
            string decodedUrl = "/";

            if(this.IsValidLoginArgument(userName, password))
            {
                var user = this.userService.Login(userName, password);

                if(user != null)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        decodedUrl = Server.UrlDecode(returnUrl);
                    if (!Url.IsLocalUrl(decodedUrl))
                        decodedUrl = "/";

                    SessionFacade.CurrentUser = user;
                    this.RedirectFromLoginPage(userName, decodedUrl);
                }
                else
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
