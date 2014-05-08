namespace DH.Helpdesk.Web.Controllers
{
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Models.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Printer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Server;
    using DH.Helpdesk.Web.Models.Inventory.SearchModels;

    using PageName = DH.Helpdesk.Web.Enums.PageName;

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
        public PartialViewResult RenderContent(int currentMode)
        {
            switch ((CurrentModes)currentMode)
            {
                case CurrentModes.Workstations:
                    return this.Workstations();

                case CurrentModes.Servers:
                    return this.Servers();

                case CurrentModes.Printers:
                    return this.Printers();

                default:
                    return this.Inventories(currentMode);
            }
        }

        [HttpGet]
        public PartialViewResult Workstations()
        {
            SessionFacade.SavePageFilters(PageName.Inventory, new CurrentModeFilter((int)CurrentModes.Workstations));
            var currentFilter = SessionFacade.FindPageFilters<WorkstationsSearchFilter>(CurrentModes.Workstations.ToString()) ?? WorkstationsSearchFilter.CreateDefault();
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
            SessionFacade.SavePageFilters(PageName.Inventory, new CurrentModeFilter((int)CurrentModes.Servers));
            var currentFilter = SessionFacade.FindPageFilters<ServerSearchFilter>(CurrentModes.Servers.ToString()) ?? ServerSearchFilter.CreateDefault();

            return this.PartialView("Servers", currentFilter);
        }

        [HttpGet]
        public PartialViewResult Printers()
        {
            SessionFacade.SavePageFilters(PageName.Inventory, new CurrentModeFilter((int)CurrentModes.Printers));
            var currentFilter = SessionFacade.FindPageFilters<PrinterSearchFilter>(CurrentModes.Printers.ToString()) ?? PrinterSearchFilter.CreateDefault();
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
            SessionFacade.SavePageFilters(PageName.Inventory, new CurrentModeFilter(inventoryTypeId));
            var currentFilter = SessionFacade.FindPageFilters<InventorySearchFilter>(inventoryTypeId.ToString(CultureInfo.InvariantCulture)) ?? InventorySearchFilter.CreateDefault(inventoryTypeId);
            var filters = this.inventoryService.GetInventoryFilters(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventoryService.GetInventoryFieldSettingsOverviewForFilter(inventoryTypeId);

            var viewModel = InventorySearchViewModel.BuildViewModel(currentFilter, filters, settings);

            return this.PartialView("Inventories", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            SessionFacade.SavePageFilters(CurrentModes.Workstations.ToString(), filter);
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
            SessionFacade.SavePageFilters(CurrentModes.Servers.ToString(), filter);
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
            SessionFacade.SavePageFilters(CurrentModes.Printers.ToString(), filter);
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
            SessionFacade.SavePageFilters(filter.InventoryTypeId.ToString(CultureInfo.InvariantCulture), filter);
            var settings = this.inventoryService.GetInventoryFieldSettingsOverview(filter.InventoryTypeId);
            var models = this.inventoryService.GetInventories(filter.CreateRequest());

            var viewModel = InventoryGridModel.BuildModel(models, settings, filter.InventoryTypeId);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpGet]
        public RedirectToRouteResult RedirectToEditInventory(int currentMode, int id)
        {
            switch ((CurrentModes)currentMode)
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
        public RedirectToRouteResult RedirectToNewInventory(int currentMode)
        {
            switch ((CurrentModes)currentMode)
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
            var activeTab = SessionFacade.FindActiveTab(Web.Enums.Inventory.PageName.ComputerEdit) ?? TabName.Computer;

            var model = this.inventoryService.GetWorkstation(id);
            var options = this.inventoryService.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventoryService.GetWorkstationFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var additionalData = this.inventoryService.GetWorkstationEditAdditionalData(
                id,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var computerEditModel = ComputerViewModel.BuildViewModel(model, options, settings);
            var inventoryGridModels = InventoryGridModel.BuildModels(
                additionalData.InventoryOverviewResponseWithType,
                additionalData.InventoriesFieldSettingsOverviewResponse);
            var inventoryTypesViewModel = DropDownViewModel.BuildViewModel(additionalData.InventoryTypes);
            var selected = additionalData.InventoryTypes.Min(x => x.Value.ToNullableInt32());
            inventoryTypesViewModel.Selected = selected;

            var viewModel = new ComputerEditViewModel(
                computerEditModel,
                additionalData.Softwaries,
                additionalData.LogicalDrives,
                additionalData.ComputerLogs,
                inventoryGridModels,
                inventoryTypesViewModel,
                activeTab);

            return this.View("EditWorkstation", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
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
        [BadRequestOnNotValid]
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
        [BadRequestOnNotValid]
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
        [BadRequestOnNotValid]
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
        public ViewResult DeleteDynamicSetting(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewWorkstation()
        {
            var options = this.inventoryService.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventoryService.GetWorkstationFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = ComputerViewModel.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id);

            return this.View("NewWorkstation", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
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
        [BadRequestOnNotValid]
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
        [BadRequestOnNotValid]
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
        [BadRequestOnNotValid]
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

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewComputerLog(ComputerLogModel computerLogModel)
        {
            this.inventoryService.AddComputerLog(computerLogModel.CreateBusinessModel());
            return this.RedirectToAction("EditWorkstation", new { id = computerLogModel.ComputerId });
        }

        [HttpGet]
        public RedirectToRouteResult DeleteComputerLog(int logId, int computerId)
        {
            this.inventoryService.DeleteComputerLog(logId);

            return this.RedirectToAction("EditWorkstation", new { id = computerId });
        }

        [HttpGet]
        public PartialViewResult SearchNotConnectedInventory(int? selected, int computerId)
        {
            if (!selected.HasValue)
            {
                return this.PartialView("DropDown", DropDownViewModel.BuildDefault());
            }

            var models = this.inventoryService.GetNotConnectedInventory(selected.Value, computerId);
            var viewModel = DropDownViewModel.BuildViewModel(models);
            return this.PartialView("DropDown", viewModel);
        }

        [HttpPost]
        public RedirectToRouteResult ConnectInventoryToComputer(int? selected, int computerId)
        {
            if (!selected.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            this.inventoryService.ConnectInventoryToComputer(selected.Value, computerId);

            return this.RedirectToAction("EditWorkstation", new { id = computerId });
        }

        [HttpGet]
        public RedirectToRouteResult DeleteInventoryFromComputer(int computerId, int inventoryId)
        {
            this.inventoryService.RemoveInventoryFromComputer(inventoryId, computerId);

            return this.RedirectToAction("EditWorkstation", new { id = computerId });
        }

        [HttpGet]
        public ActionResult SearchDepartmentsByRegionId(int? selected)
        {
            var models = this.inventoryService.GetDepartments(SessionFacade.CurrentCustomer.Id, selected);
            var currentFilter = SessionFacade.FindPageFilters<WorkstationsSearchFilter>(CurrentModes.Workstations.ToString()) ?? WorkstationsSearchFilter.CreateDefault();

            var viewModel = DropDownViewModel.BuildViewModel(models);
            viewModel.AllowEmpty = true;
            viewModel.PropertyName = DropDownName.DepartmentName;
            viewModel.Selected = currentFilter.DepartmentId;
            return this.PartialView("DropDown", viewModel);
        }
    }
}
