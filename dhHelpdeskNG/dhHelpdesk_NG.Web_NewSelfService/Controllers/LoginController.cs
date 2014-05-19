namespace DH.Helpdesk.NewSelfService.Controllers
{
    using System.Web.Mvc;

    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return new RedirectResult(HttpContext.Request.QueryString["ReturnUrl"]);
        }
    }
}