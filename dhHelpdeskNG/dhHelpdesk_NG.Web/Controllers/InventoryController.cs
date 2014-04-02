namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

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
                    return this.Inventories((int)currentMode);
            }
        }

        [HttpGet]
        public PartialViewResult Workstations()
        {
            var currentFilter = SessionFacade.GetPageFilters<WorkstationsSearchFilter>(Enums.PageName.Inventory) ?? WorkstationsSearchFilter.CreateDefault(SessionFacade.CurrentCustomer.Id);
            var filters = this.inventoryService.GetWorkstationFilters(SessionFacade.CurrentCustomer.Id);

            // todo maybe, would be better using custom settings aggregate for filter only with necessary fields
            var settings = this.inventoryService.GetWorkstationFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = WorkstationSearchViewModel.BuildViewModel(currentFilter, filters, settings);

            return this.PartialView("Workstations", viewModel);
        }

        [HttpGet]
        public PartialViewResult Servers()
        {
            var currentFilter = SessionFacade.GetPageFilters<ServerSearchFilter>(Enums.PageName.Inventory) ?? ServerSearchFilter.CreateDefault(SessionFacade.CurrentCustomer.Id);

            return this.PartialView("Servers", currentFilter);
        }

        [HttpGet]
        public PartialViewResult Printers()
        {
            var currentFilter = SessionFacade.GetPageFilters<PrinterSearchFilter>(Enums.PageName.Inventory) ?? PrinterSearchFilter.CreateDefault(SessionFacade.CurrentCustomer.Id);
            var filters = this.inventoryService.GetPrinterFilters(SessionFacade.CurrentCustomer.Id);

            // todo maybe, would be better using custom settings aggregate for filter only with necessary fields
            var settings = this.inventoryService.GetPrinterFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = PrinterSearchViewModel.BuildViewModel(currentFilter, filters, settings);

            return this.PartialView("Printers", viewModel);
        }

        [HttpGet]
        public PartialViewResult Inventories(int inventoryTypeId)
        {
            var currentFilter = SessionFacade.GetPageFilters<InventorySearchFilter>(Enums.PageName.Inventory) ?? InventorySearchFilter.CreateDefault(SessionFacade.CurrentCustomer.Id);
            var filters = this.inventoryService.GetInventoryFilters(SessionFacade.CurrentCustomer.Id);

            // todo maybe, would be better using custom settings aggregate for filter only with necessary fields
            var settings = this.inventoryService.GetInventoryFieldSettingsOverview(SessionFacade.CurrentCustomer.Id, inventoryTypeId);

            var viewModel = InventorySearchViewModel.BuildViewModel(currentFilter, filters, settings);


            return this.PartialView("Inventories", viewModel);
        }

        [HttpPost]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            var settings = this.inventoryService.GetWorkstationFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetWorkstations(filter.CreateRequest());

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpPost]
        public PartialViewResult ServersGrid(ServerSearchFilter filter)
        {
            var settings = this.inventoryService.GetServerFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetServers(filter.CreateRequest());

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpPost]
        public PartialViewResult PrintersGrid(PrinterSearchFilter filter)
        {
            var settings = this.inventoryService.GetPrinterFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetPrinters(filter.CreateRequest());

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpPost]
        public PartialViewResult InventoriesGrid(InventorySearchFilter filter)
        {
            var settings = this.inventoryService.GetInventoryFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetInventories(filter.CreateRequest());

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);

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
