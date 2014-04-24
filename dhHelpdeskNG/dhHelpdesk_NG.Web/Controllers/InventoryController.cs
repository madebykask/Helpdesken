namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Printer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Server;
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
            var currentModeFilter = SessionFacade.FindPageFilters<CurrentModeFilter>(PageName.Inventory) ?? CurrentModeFilter.GetDefault();
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
            var currentFilter = SessionFacade.FindPageFilters<WorkstationsSearchFilter>(PageName.Inventory) ?? WorkstationsSearchFilter.CreateDefault();
            var filters = this.inventoryService.GetWorkstationFilters(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventoryService.GetWorkstationFieldSettingsOverviewForFilter(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = WorkstationSearchViewModel.BuildViewModel(currentFilter, filters, settings);

            return this.PartialView("Workstations", viewModel);
        }

        [HttpGet]
        public PartialViewResult Servers()
        {
            var currentFilter = SessionFacade.FindPageFilters<ServerSearchFilter>(PageName.Inventory) ?? ServerSearchFilter.CreateDefault();

            return this.PartialView("Servers", currentFilter);
        }

        [HttpGet]
        public PartialViewResult Printers()
        {
            var currentFilter = SessionFacade.FindPageFilters<PrinterSearchFilter>(PageName.Inventory) ?? PrinterSearchFilter.CreateDefault();
            var filters = this.inventoryService.GetPrinterFilters(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventoryService.GetPrinterFieldSettingsOverviewForFilter(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = PrinterSearchViewModel.BuildViewModel(currentFilter, filters, settings);

            return this.PartialView("Printers", viewModel);
        }

        [HttpGet]
        public PartialViewResult Inventories(int inventoryTypeId)
        {
            var currentFilter = SessionFacade.FindPageFilters<InventorySearchFilter>(PageName.Inventory) ?? InventorySearchFilter.CreateDefault(SessionFacade.CurrentCustomer.Id);
            var filters = this.inventoryService.GetInventoryFilters(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventoryService.GetInventoryFieldSettingsOverviewForFilter(inventoryTypeId);

            var viewModel = InventorySearchViewModel.BuildViewModel(currentFilter, filters, settings);

            return this.PartialView("Inventories", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            var settings = this.inventoryService.GetWorkstationFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetWorkstations(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ServersGrid(ServerSearchFilter filter)
        {
            var settings = this.inventoryService.GetServerFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetServers(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult PrintersGrid(PrinterSearchFilter filter)
        {
            var settings = this.inventoryService.GetPrinterFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetPrinters(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoriesGrid(InventorySearchFilter filter)
        {
            var settings = this.inventoryService.GetInventoryFieldSettingsOverview(
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
        public RedirectToRouteResult NewInventory(CurrentModes currentMode)
        {
            switch (currentMode)
            {
                case CurrentModes.Workstations:
                    return this.RedirectToAction("NewWorkstation");

                case CurrentModes.Servers:
                    return this.RedirectToAction("NewServer");

                case CurrentModes.Printers:
                    return this.RedirectToAction("NewPrinter");

                default:
                    return this.RedirectToAction("NewInventory");
            }
        }

        [HttpGet]
        public ViewResult EditWorkstation(int id)
        {
            return this.View("EditWorkstation");
        }

        [HttpPost]
        public ViewResult EditWorkstation(ComputerViewModel computerViewModel)
        {
            return this.View("EditWorkstation");
        }

        [HttpGet]
        public ViewResult EditServer(int id)
        {
            return this.View("EditServer");
        }

        [HttpPost]
        public ViewResult EditServer(ServerViewModel serverViewModel)
        {
            return this.View("EditServer");
        }

        [HttpGet]
        public ViewResult EditPrinter(int id)
        {
            return this.View("EditPrinter");
        }

        [HttpPost]
        public ViewResult EditPrinter(PrinterViewModel printerViewModel)
        {
            return this.View("EditPrinter");
        }

        [HttpGet]
        public ViewResult EditInventory(int id)
        {
            return this.View("EditInventory");
        }

        [HttpPost]
        public ViewResult EditInventory(InventoryViewModel inventoryViewModel)
        {
            return this.View("EditInventory");
        }

        [HttpGet]
        public ViewResult DeleteWorkstation(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeleteServer(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeletePrinter(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeleteInventory(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeleteComputerLog(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeleteDynamicSetting(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewWorkstation()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public ViewResult NewWorkstation(ComputerViewModel computerViewModel)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewServer()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public ViewResult NewServer(ServerViewModel serverViewModel)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewPrinter()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public ViewResult NewPrinter(PrinterViewModel printerViewModel)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewInventory()
        {
            throw new System.NotImplementedException();
        }

        [HttpPost]
        public ViewResult NewInventory(InventoryViewModel inventoryViewModel)
        {
            throw new System.NotImplementedException();
        }

        public ActionResult EditComputerModule()
        {
            throw new System.NotImplementedException();
        }

        public ActionResult NewComputerModule()
        {
            throw new System.NotImplementedException();
        }
    }
}
