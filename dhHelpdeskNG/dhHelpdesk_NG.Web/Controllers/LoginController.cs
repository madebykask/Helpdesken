using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Web.Infrastructure;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LoginController(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult Login()
        {
            Session.Clear();
            FormsAuthentication.SignOut();

#if DEBUG
            var u = "mj";
            var p = "maj";

            var user = _userRepository.Login(u, p);

            if(user != null)
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
                var user = _userRepository.Login(userName, password);

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
