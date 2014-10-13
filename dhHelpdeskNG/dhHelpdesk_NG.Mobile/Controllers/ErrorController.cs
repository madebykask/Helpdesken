namespace DH.Helpdesk.Mobile.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Infrastructure;

    public class ErrorController : BaseController
    {
        public ErrorController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        public ActionResult Index()
        {
            return this.View("Error");
        }

        public ActionResult NotFound()
        {
            return this.View("Error");
        }

        public ViewResult BusinessLogicError()
        {
            return this.View();
        }
    }
}
