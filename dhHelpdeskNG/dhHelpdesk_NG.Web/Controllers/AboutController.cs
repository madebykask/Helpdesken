namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class AboutController : BaseController
    {
        public AboutController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        [HttpGet]
        public ViewResult Index()
        {
            return this.View();
        }
    }
}
