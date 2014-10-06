namespace DH.Helpdesk.Mobile.Areas.Admin.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Mobile.Infrastructure;

    public class StartController : BaseController
    {
        public StartController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        public ActionResult Index()
        {
            return this.View();
        }
    }
}
