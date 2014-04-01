namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.SearchModels;

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

        public ViewResult Index()
        {
            var currentModeFilter = SessionFacade.GetPageFilters<CurrentModeFilter>(Enums.PageName.Inventory) ?? CurrentModeFilter.GetDefault();
            var inventoryTypes = this.inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id);

            var viewModel = IndexViewModel.BuildViewModel(currentModeFilter.CurrentMode, inventoryTypes);

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
                    return this.Inventories();
            }
        }

        [HttpGet]
        public PartialViewResult Workstations()
        {
            // todo should be clien filter in session
            var currentFilter = SessionFacade.GetPageFilters<ComputersFilter>(Enums.PageName.Inventory) ?? ComputersFilter.CreateDefault();
            var filters = this.inventoryService.GetWorkstationFilters(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventoryService.GetWorkstationFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = WorkstationSearchViewModel.BuildViewModel(currentFilter, filters, settings);

            return this.PartialView("Workstations", viewModel);
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
        public PartialViewResult Inventories()
        {
            return this.PartialView("Inventories");
        }

        [HttpPost]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            var settings = this.inventoryService.GetWorkstationFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            //var models = this.inventoryService.GetWorkstations(filter);

            //var gridViewModel = InventoryGridModel.BuildModel(models, settings);

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

        [HttpGet]
        public RedirectToRouteResult EditInventory(CurrentModes currentMode, int id)
        {
            switch (currentMode)
            {
                case CurrentModes.Workstations:
                    return this.RedirectToAction("EditWorkstation", new { id });

                case CurrentModes.Servers:
                    return this.RedirectToAction("EditServer", new { id });

                case CurrentModes.Printers:
                    return this.RedirectToAction("EditPrinter", new { id });

                default:
                    return this.RedirectToAction("EditInventory", new { id });
            }
        }

        [HttpGet]
        public ViewResult EditWorkstation(int id)
        {
            return this.View("EditWorkstation");
        }

        [HttpGet]
        public ViewResult EditServer(int id)
        {
            return this.View("EditServer");
        }

        [HttpGet]
        public ViewResult EditPrinter(int id)
        {
            return this.View("EditPrinter");
        }

        [HttpGet]
        public ViewResult EditInventory(int id)
        {
            return this.View("EditInventory");
        }
    }
}
