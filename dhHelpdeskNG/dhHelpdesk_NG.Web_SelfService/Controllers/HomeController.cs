using System.Security.Principal;
using System.Threading;
using DH.Helpdesk.SelfService.Infrastructure.Configuration;

namespace DH.Helpdesk.SelfService.Controllers
{
    using System.Web.Mvc;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.SelfService.Infrastructure;
    using System.Web;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OpenIdConnect;
    using Microsoft.Owin.Security.Cookies;

    public class HomeController : BaseController
    {                
        public HomeController( IMasterDataService masterDataService,
                               ISelfServiceConfigurationService configurationService,
                               ICaseSolutionService caseSolutionService                        
                              ) : base(configurationService, masterDataService, caseSolutionService)
            
        {
            //this._customerService = customerService;
        }

        public RedirectToRouteResult Index(int customerId = -1)
        {
            return RedirectToAction("Index", "Start", new { customerId });
        }

        //diagnostic action
        public JsonResult _Ctx()
        {
            var winIdentity = WindowsIdentity.GetCurrent();

            return Json(new
            {
                HttpContextUserName = ControllerContext.RequestContext.HttpContext.User.Identity.Name,
                HttpContextUserType = ControllerContext.RequestContext.HttpContext.User.Identity.GetType().Name,
                WinIdentity = winIdentity != null ? $"{winIdentity.Name} | {winIdentity.AuthenticationType}" : "None",
                ThreadUser = Thread.CurrentPrincipal?.Identity?.Name ?? "None"
            }, JsonRequestBehavior.AllowGet);

        }
    }
}
