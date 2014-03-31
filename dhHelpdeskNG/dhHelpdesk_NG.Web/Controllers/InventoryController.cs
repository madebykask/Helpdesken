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

        [HttpPost]
        public RedirectToRouteResult RenderContent(CurrentModes currentMode)
        {
            switch (currentMode)
            {
                case CurrentModes.Workstations:
                    return this.RedirectToAction("Workstations");

                case CurrentModes.Servers:
                    return this.RedirectToAction("Servers");

                case CurrentModes.Printers:
                    return this.RedirectToAction("Printers");

                default:
                    return this.RedirectToAction("DynamicTypes");
            }
        }

        [HttpGet]
        public PartialViewResult Workstations()
        {
            return null;
        }

        [HttpGet]
        public PartialViewResult Servers()
        {
            return null;
        }

        [HttpGet]
        public PartialViewResult Printers()
        {
            return null;
        }

        [HttpGet]
        public PartialViewResult DynamicTypes()
        {
            return null;
        }
    }
}
