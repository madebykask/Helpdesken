namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models.Inventory;

    public class InventoryController : BaseController
    {
        private readonly IInventoryService inventoryService;

        public InventoryController(
            IMasterDataService masterDataService,
            IInventoryService inventoryService)
            : base(masterDataService)
        {
            this.inventoryService = inventoryService;
        }

        public ActionResult Index()
        {
            var currentModeFilter = SessionFacade.GetPageFilters<CurrentModeFilter>(Enums.PageName.Inventory) ?? CurrentModeFilter.GetDefaultFilter();
            var inventoryTypes = this.inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id);

            var viewModel = IndexViewModel.GetModel(currentModeFilter.CurrentMode, inventoryTypes);

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult RenderContent(CurrentModes currentMode)
        {
            switch (currentMode)
            {
                case CurrentModes.Workstations:
                    return this.Workstations();

                case CurrentModes.Servers:
                    return this.Servers();

                case CurrentModes.Printers:
                    return this.Printers();

                default:
                    return this.DynamicTypes();
            }
        }

        [HttpGet]
        public PartialViewResult Workstations()
        {
            return this.PartialView("Workstations");
        }

        [HttpGet]
        public PartialViewResult Servers()
        {
            return this.PartialView("Servers");
        }

        [HttpGet]
        public PartialViewResult Printers()
        {
            return this.PartialView("Printers");
        }

        [HttpGet]
        public PartialViewResult DynamicTypes()
        {
            return this.PartialView("DynamicTypes");
        }

        [HttpPost]
        public PartialViewResult WorkstationsGrid()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public PartialViewResult ServersGrid()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public PartialViewResult PrintersGrid()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public PartialViewResult InventoriesGrid()
        {
            throw new System.NotImplementedException();
        }
    }
}
