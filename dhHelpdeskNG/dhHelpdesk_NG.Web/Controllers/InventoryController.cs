namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;

    public class InventoryController : BaseController
    {
        public InventoryController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        public ActionResult Index()
        {
            throw new NotImplementedException();
        }
    }
}
