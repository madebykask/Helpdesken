using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Controllers
{
    using dhHelpdesk_NG.Service;

    public class LoginController : Controller
    {
        private readonly IUserService userService;

        public LoginController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Login()
        {
            Session.Clear();
            FormsAuthentication.SignOut();

#if DEBUG
            var u = "mj";
            var p = "maj";

            var user = this.userService.Login(u, p);

            if (user != null)
            {
                SessionFacade.CurrentUser = user;
                this.RedirectFromLoginPage(u, null);
            }
#endif

            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection coll)
        {
            string userName = coll["txtUid"].ToString().Trim();
            string password = coll["txtPwd"].ToString().Trim();
            string returnURL = "/";

            if(this.IsValidLoginArgument(userName, password))
            {
                var user = this.userService.Login(userName, password);

                if(user != null)
                {
                    SessionFacade.CurrentUser = user;
                    this.RedirectFromLoginPage(userName, returnURL);
                }
                else
                    TempData["LoginFailed"] = "Login failed! The user name or password entered is incorrect.";
            }

            return View("Login");
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

            Response.Cookies.Add(cookie);

            if(!string.IsNullOrEmpty(returnURL))
                Response.Redirect(returnURL);
            else
                Response.Redirect(FormsAuthentication.DefaultUrl);
        }
    }
}
