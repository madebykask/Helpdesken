namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;
    using DH.Helpdesk.Web.Models.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Computer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Printer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Server;
    using DH.Helpdesk.Web.Models.Inventory.OptionsAggregates;
    using DH.Helpdesk.Web.Models.Inventory.SearchModels;

    public class InventoryController : BaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IInventorySettingsService inventorySettingsService;

        private readonly IComputerModulesService computerModulesService;

        private readonly IOrganizationService organizationService;

        private readonly IPlaceService placeService;

        private readonly IComputerViewModelBuilder computerViewModelBuilder;

        private readonly IServerViewModelBuilder serverViewModelBuilder;

        private readonly IPrinterViewModelBuilder printerViewModelBuilder;

        private readonly IInventoryViewModelBuilder inventoryViewModelBuilder;

        private readonly IDynamicsFieldsModelBuilder dynamicsFieldsModelBuilder;

        public InventoryController(
            IMasterDataService masterDataService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService,
            IComputerModulesService computerModulesService,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IComputerViewModelBuilder computerViewModelBuilder,
            IServerViewModelBuilder serverViewModelBuilder,
            IPrinterViewModelBuilder printerViewModelBuilder,
            IInventoryViewModelBuilder inventoryViewModelBuilder,
            IDynamicsFieldsModelBuilder dynamicsFieldsModelBuilder)
            : base(masterDataService)
        {
            this.inventoryService = inventoryService;
            this.inventorySettingsService = inventorySettingsService;
            this.computerModulesService = computerModulesService;
            this.organizationService = organizationService;
            this.placeService = placeService;
            this.computerViewModelBuilder = computerViewModelBuilder;
            this.serverViewModelBuilder = serverViewModelBuilder;
            this.printerViewModelBuilder = printerViewModelBuilder;
            this.inventoryViewModelBuilder = inventoryViewModelBuilder;
            this.dynamicsFieldsModelBuilder = dynamicsFieldsModelBuilder;
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")] // todo
        public ViewResult Index()
        {
            var activeTab = SessionFacade.FindActiveTab(PageName.InventoryIndex) ?? TabName.Inventories;

            var currentModeFilter = SessionFacade.FindPageFilters<InventoriesModeFilter>(TabName.Inventories) ?? InventoriesModeFilter.GetDefault();
            var reportTypeFilter = SessionFacade.FindPageFilters<ReportFilter>(TabName.Reports) ?? ReportFilter.GetDefault();
            var moduleTypeFilter = SessionFacade.FindPageFilters<ComputerModuleModeFilter>(TabName.MasterData) ?? ComputerModuleModeFilter.GetDefault();

            var inventoryTypes = this.inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id);

            var viewModel = IndexViewModel.BuildViewModel(
                currentModeFilter.CurrentMode,
                reportTypeFilter.ReportType,
                moduleTypeFilter.ModuleType,
                inventoryTypes,
                activeTab);

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult RenderContent(int currentMode)
        {
            switch ((CurrentModes)currentMode)
            {
                case CurrentModes.Workstations:
                    return this.Workstations(currentMode);

                case CurrentModes.Servers:
                    return this.Servers(currentMode);

                case CurrentModes.Printers:
                    return this.Printers(currentMode);

                default:
                    return this.Inventories(currentMode);
            }
        }

        [HttpGet]
        public PartialViewResult Workstations(int currentModeId)
        {
            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter(currentModeId));
            var currentFilter =
                SessionFacade.FindPageFilters<WorkstationsSearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Workstation.ToString()))
                ?? WorkstationsSearchFilter.CreateDefault();

            var computerTypes = this.computerModulesService.GetComputerTypes(SessionFacade.CurrentCustomer.Id);
            var regions = this.organizationService.GetRegions(SessionFacade.CurrentCustomer.Id);
            var departments = this.organizationService.GetDepartments(
                SessionFacade.CurrentCustomer.Id,
                currentFilter.RegionId);
            var settings = this.inventorySettingsService.GetWorkstationFieldSettingsOverviewForFilter(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = WorkstationSearchViewModel.BuildViewModel(
                currentFilter,
                regions,
                departments,
                computerTypes,
                settings);

            return this.PartialView("Workstations", viewModel);
        }

        [HttpGet]
        public PartialViewResult Servers(int currentModeId)
        {
            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter(currentModeId));
            var currentFilter =
                SessionFacade.FindPageFilters<ServerSearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Server.ToString()))
                ?? ServerSearchFilter.CreateDefault();

            return this.PartialView("Servers", currentFilter);
        }

        [HttpGet]
        public PartialViewResult Printers(int currentModeId)
        {
            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter(currentModeId));
            var currentFilter =
                SessionFacade.FindPageFilters<PrinterSearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Printer.ToString()))
                ?? PrinterSearchFilter.CreateDefault();

            var departments = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetPrinterFieldSettingsOverviewForFilter(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = PrinterSearchViewModel.BuildViewModel(currentFilter, departments, settings);

            return this.PartialView("Printers", viewModel);
        }

        [HttpGet]
        public PartialViewResult Inventories(int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter(inventoryTypeId));
            var currentFilter =
                SessionFacade.FindPageFilters<InventorySearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.CustomType.ToString()))
                ?? InventorySearchFilter.CreateDefault(inventoryTypeId);

            var departments = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetInventoryFieldSettingsOverviewForFilter(inventoryTypeId);

            var viewModel = InventorySearchViewModel.BuildViewModel(currentFilter, departments, settings, inventoryTypeId);

            return this.PartialView("Inventories", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Workstation.ToString()),
                filter);

            var settings = this.inventorySettingsService.GetWorkstationFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetWorkstations(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ServersGrid(ServerSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Server.ToString()),
                filter);

            var settings = this.inventorySettingsService.GetServerFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetServers(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult PrintersGrid(PrinterSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Printer.ToString()),
                filter);

            var settings = this.inventorySettingsService.GetPrinterFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetPrinters(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoriesGrid(InventorySearchFilter filter, int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.CustomType.ToString()),
                filter);

            var settings = this.inventorySettingsService.GetInventoryFieldSettingsOverview(inventoryTypeId);
            var models = this.inventoryService.GetInventories(filter.CreateRequest(inventoryTypeId));

            var viewModel = InventoryGridModel.BuildModel(models, settings, inventoryTypeId);

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
                    return this.RedirectToAction(
                        "EditInventory",
                        new { id, inventoryTypeId = currentMode });
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
                    return this.RedirectToAction("NewInventory", new { inventoryTypeId = currentMode });
            }
        }

        [HttpGet]
        public ViewResult EditWorkstation(int id)
        {
            var activeTab = SessionFacade.FindActiveTab(PageName.ComputerEdit) ?? TabName.Computer;

            var model = this.inventoryService.GetWorkstation(id);
            var options = this.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var softwares = this.computerModulesService.GetComputerSoftware(id);
            var drives = this.computerModulesService.GetComputerLogicalDrive(id);
            var logs = this.inventoryService.GetWorkstationLogOverviews(id);
            var computerEditModel = this.computerViewModelBuilder.BuildViewModel(model, options, settings);

            var viewModel = new ComputerEditViewModel(
                computerEditModel,
                softwares,
                drives,
                logs,
                activeTab);

            return this.View("EditWorkstation", viewModel);
        }

        [HttpGet]
        public PartialViewResult RenderAccesories(int computerId)
        {
            var inventory = this.inventoryService.GetConnectedToComputerInventories(computerId);
            var invetoryTypeIds = inventory.Overviews.Select(x => x.InventoryTypeId).ToList();
            var settings = this.inventorySettingsService.GetInventoryFieldSettingsOverview(invetoryTypeIds);
            var inventoryGridModels = InventoryGridModel.BuildModels(inventory, settings);

            var inventoryTypes = this.inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id);

            // todo
            var selected = inventoryTypes.Min(x => x.Value.ToNullableInt32());
            var inventories = selected.HasValue
                                  ? this.inventoryService.GetNotConnectedInventory(
                                      selected.Value,
                                      computerId)
                                  : new List<ItemOverview>();

            var viewModel = AccesoriesViewModel.BuildViewModel(
                computerId,
                selected,
                inventoryTypes,
                inventories,
                inventoryGridModels);

            return this.PartialView("Accesories", viewModel);
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
            var model = this.inventoryService.GetServer(id);
            var options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetServerFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var softwares = this.computerModulesService.GetServerSoftware(id);
            var drives = this.computerModulesService.GetServerLogicalDrive(id);
            var logs = this.inventoryService.GetOperationServerLogOverviews(id, SessionFacade.CurrentCustomer.Id);

            var serverEditModel = this.serverViewModelBuilder.BuildViewModel(model, options, settings);

            var viewModel = new ServerEditViewModel(
                serverEditModel,
                softwares,
                drives,
                logs);

            return this.View("EditServer", viewModel);
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
            var model = this.inventoryService.GetPrinter(id);
            var options = this.GetPrinterEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetPrinterFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var printerEditModel = this.printerViewModelBuilder.BuildViewModel(model, options, settings);

            return this.View("EditPrinter", printerEditModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult EditPrinter(PrinterViewModel printerViewModel)
        {
            return this.View("EditPrinter");
        }

        [HttpGet]
        public ViewResult EditInventory(int id, int inventoryTypeId)
        {
            var model = this.inventoryService.GetInventory(id);
            var settings = this.inventorySettingsService.GetInventoryFieldSettingsForModelEdit(inventoryTypeId);
            var options = this.GetInventoryInventoryEditOptions(SessionFacade.CurrentCustomer.Id);
            var typeGroupModels = this.inventoryService.GetTypeGroupModels(inventoryTypeId);

            var inventoryViewModel = this.inventoryViewModelBuilder.BuildViewModel(
                model.Inventory,
                options,
                settings.InventoryFieldSettingsForModelEdit);
            var dynamicFieldsModel = this.dynamicsFieldsModelBuilder.BuildViewModel(
                model.DynamicData,
                settings.InventoryDynamicFieldSettingForModelEditData,
                id);
            inventoryViewModel.Name = model.Inventory.InventoryTypeName;

            var viewModel = new InventoryEditViewModel(inventoryViewModel, dynamicFieldsModel, typeGroupModels);

            return this.View("EditInventory", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult EditInventory(InventoryEditViewModel inventoryEditViewModel)
        {
            return this.View("EditInventory");
        }

        [HttpGet]
        public ViewResult DeleteWorkstation(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeleteServer(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeletePrinter(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeleteInventory(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewWorkstation()
        {
            var options = this.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = this.computerViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id);

            return this.View("NewWorkstation", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult NewWorkstation(ComputerViewModel computerViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewServer()
        {
            var options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetServerFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var viewModel = this.serverViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id);

            return this.View("NewServer", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult NewServer(ServerViewModel serverViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewPrinter()
        {
            var options = this.GetPrinterEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetPrinterFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var printerEditModel = this.printerViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id);

            return this.View("NewPrinter", printerEditModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult NewPrinter(PrinterViewModel printerViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewInventory(int inventoryTypeId)
        {
            var inventoryType = this.inventoryService.GetInventoryType(inventoryTypeId);
            var settings = this.inventorySettingsService.GetInventoryFieldSettingsForModelEdit(inventoryTypeId);
            var options = this.GetInventoryInventoryEditOptions(SessionFacade.CurrentCustomer.Id);
            var typeGroupModels = this.inventoryService.GetTypeGroupModels(inventoryTypeId);

            var inventoryViewModel = this.inventoryViewModelBuilder.BuildViewModel(
                options,
                settings.InventoryFieldSettingsForModelEdit,
                inventoryTypeId,
                SessionFacade.CurrentCustomer.Id);
            var dynamicFieldsModel =
                this.dynamicsFieldsModelBuilder.BuildViewModel(settings.InventoryDynamicFieldSettingForModelEditData);
            inventoryViewModel.Name = inventoryType.Name;

            var viewModel = new InventoryEditViewModel(inventoryViewModel, dynamicFieldsModel, typeGroupModels);

            return this.View("EditInventory", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult NewInventory(InventoryEditViewModel inventoryEditViewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult EditComputerModule()
        {
            throw new NotImplementedException();
        }

        public ActionResult NewComputerModule()
        {
            throw new NotImplementedException();
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
        public JsonResult SearchNotConnectedInventory(int? selected, int computerId)
        {
            if (!selected.HasValue)
            {
                return this.Json(new { });
            }

            var models = this.inventoryService.GetNotConnectedInventory(selected.Value, computerId);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public RedirectToRouteResult ConnectInventoryToComputer(int? inventoryId, int computerId)
        {
            if (!inventoryId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            this.inventoryService.ConnectInventoryToComputer(inventoryId.Value, computerId);

            return this.RedirectToAction("EditWorkstation", new { id = computerId });
        }

        [HttpGet]
        public RedirectToRouteResult DeleteInventoryFromComputer(int computerId, int inventoryId)
        {
            this.inventoryService.RemoveInventoryFromComputer(inventoryId, computerId);

            return this.RedirectToAction("EditWorkstation", new { id = computerId });
        }

        [HttpGet]
        public JsonResult SearchDepartmentsByRegionId(int? selected)
        {
            var models = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id, selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchFloorsByBuildingId(int? selected)
        {
            var models = this.placeService.GetFloors(SessionFacade.CurrentCustomer.Id, selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchRoomsByFloorId(int? selected)
        {
            var models = this.placeService.GetRooms(SessionFacade.CurrentCustomer.Id, selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult RenderCategoryContent(int moduleType)
        {
            SessionFacade.SavePageFilters(TabName.MasterData, new ComputerModuleModeFilter(moduleType));
            var items = new List<ItemOverview>();

            switch ((ModuleTypes)moduleType)
            {
                case ModuleTypes.ComputerModel:
                    items = this.computerModulesService.GetComputerModels();
                    break;
                case ModuleTypes.ComputerType:
                    items = this.computerModulesService.GetComputerTypes(SessionFacade.CurrentCustomer.Id);
                    break;
                case ModuleTypes.Processor:
                    items = this.computerModulesService.GetProcessors();
                    break;
                case ModuleTypes.OperatingSystem:
                    items = this.computerModulesService.GetOperatingSystems();
                    break;
                case ModuleTypes.Ram:
                    items = this.computerModulesService.GetRams();
                    break;
                case ModuleTypes.NetworkAdapter:
                    items = this.computerModulesService.GetNetAdapters();
                    break;
            }

            var viewModel = new ComputerModuleGridModel(items.OrderBy(x => x.Name).ToList(), (ModuleTypes)moduleType);

            return this.PartialView("ModuleGrid", viewModel);
        }

        [HttpGet]
        public ViewResult EditComputerModule(int id, string name, ModuleTypes moduleType)
        {
            var viewModel = new ComputerModuleEditModel(id, name, moduleType);

            return this.View("EditComputerModule", viewModel);
        }

        [HttpPost]
        public RedirectToRouteResult EditComputerModule(ComputerModuleEditModel model)
        {
            var businessModel = ComputerModule.CreateUpdated(model.Id, model.Name, DateTime.Now);

            switch (model.ModuleType)
            {
                case ModuleTypes.ComputerModel:
                    this.computerModulesService.UpdateComputerModel(businessModel);
                    break;
                case ModuleTypes.ComputerType:
                    this.computerModulesService.UpdateComputerType(businessModel);
                    break;
                case ModuleTypes.Processor:
                    this.computerModulesService.UpdateProcessor(businessModel);
                    break;
                case ModuleTypes.OperatingSystem:
                    this.computerModulesService.UpdateOperatingSystem(businessModel);
                    break;
                case ModuleTypes.Ram:
                    this.computerModulesService.UpdateRam(businessModel);
                    break;
                case ModuleTypes.NetworkAdapter:
                    this.computerModulesService.UpdateNetAdapter(businessModel);
                    break;
            }

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewComputerModule(ModuleTypes moduleType)
        {
            var viewModel = new ComputerModuleEditModel(moduleType);

            return this.View("NewComputerModule", viewModel);
        }

        [HttpPost]
        public RedirectToRouteResult NewComputerModule(ComputerModuleEditModel model)
        {
            var businessModel = ComputerModule.CreateNew(model.Name, DateTime.Now);

            switch (model.ModuleType)
            {
                case ModuleTypes.ComputerModel:
                    this.computerModulesService.AddComputerModel(businessModel);
                    break;
                case ModuleTypes.ComputerType:
                    this.computerModulesService.AddComputerType(businessModel);
                    break;
                case ModuleTypes.Processor:
                    this.computerModulesService.AddProcessor(businessModel);
                    break;
                case ModuleTypes.OperatingSystem:
                    this.computerModulesService.AddOperatingSystem(businessModel);
                    break;
                case ModuleTypes.Ram:
                    this.computerModulesService.AddRam(businessModel);
                    break;
                case ModuleTypes.NetworkAdapter:
                    this.computerModulesService.AddNetAdapter(businessModel);
                    break;
            }

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult DeleteComputerModule(int id, ModuleTypes moduleType)
        {
            switch (moduleType)
            {
                case ModuleTypes.ComputerModel:
                    this.computerModulesService.DeleteComputerModel(id);
                    break;
                case ModuleTypes.ComputerType:
                    this.computerModulesService.DeleteComputerType(id);
                    break;
                case ModuleTypes.Processor:
                    this.computerModulesService.DeleteProcessor(id);
                    break;
                case ModuleTypes.OperatingSystem:
                    this.computerModulesService.DeleteOperatingSystem(id);
                    break;
                case ModuleTypes.Ram:
                    this.computerModulesService.DeleteRam(id);
                    break;
                case ModuleTypes.NetworkAdapter:
                    this.computerModulesService.DeleteNetAdapter(id);
                    break;
            }

            return this.RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult RenderReports(int reportType)
        {
            switch ((ReportTypes)reportType)
            {
                case ReportTypes.OperatingSystem:
                case ReportTypes.ServicePack:
                case ReportTypes.Processor:
                case ReportTypes.Ram:
                case ReportTypes.NetworkAdapter:
                case ReportTypes.Location:
                    return this.Reports(reportType);
                case ReportTypes.InstaledPrograms:
                    return this.InstaledProgramsReport(reportType);
                case ReportTypes.Inventory:
                    return this.InventoryReport(reportType);
                default:
                    return this.CustomTypeReport(reportType);
            }
        }

        [HttpGet]
        public PartialViewResult InstaledProgramsReport(int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(TabName.Reports, new ReportFilter(inventoryTypeId));
            var currentFilter =
                SessionFacade.FindPageFilters<InstaledProgramsReportSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.InstaledPrograms.ToString()))
                ?? InstaledProgramsReportSearchFilter.CreateDefault();

            var departments = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);

            var viewModel = InstaledProgramsReportSearchViewModel.BuildViewModel(
                currentFilter,
                departments);

            return this.PartialView("InstaledProgramsReport", viewModel);
        }

        [HttpGet]
        public PartialViewResult Reports(int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(TabName.Reports, new ReportFilter(inventoryTypeId));
            var currentFilter =
                SessionFacade.FindPageFilters<ReportsSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.DefaultReport.ToString()))
                ?? ReportsSearchFilter.CreateDefault();

            var departments = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);

            var viewModel = ReportsSearchViewModel.BuildViewModel(currentFilter, departments, inventoryTypeId);

            return this.PartialView("Reports", viewModel);
        }

        [HttpGet]
        public PartialViewResult CustomTypeReport(int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(TabName.Reports, new ReportFilter(inventoryTypeId));
            var currentFilter =
                SessionFacade.FindPageFilters<CustomTypeReportsSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.CustomType.ToString()))
                ?? CustomTypeReportsSearchFilter.CreateDefault();

            var departments = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);

            var viewModel = CustomTypeReportSearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                inventoryTypeId);

            return this.PartialView("CustomTypeReport", viewModel);
        }

        [HttpGet]
        public PartialViewResult InventoryReport(int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(TabName.Reports, new ReportFilter(inventoryTypeId));
            var currentFilter =
                SessionFacade.FindPageFilters<InventoryReportSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.Inventory.ToString()))
                ?? InventoryReportSearchFilter.CreateDefault();

            var departments = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);

            var viewModel = InventoryReportSearchViewModel.BuildViewModel(
                currentFilter,
                departments);

            return this.PartialView("InventoryReport", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InstaledProgramsReportGrid(InstaledProgramsReportSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Reports, ReportFilterMode.InstaledPrograms.ToString()),
                filter);

            if (filter.IsShowMissingParentInventory)
            {
                return null;
            }

            var viewModel = this.BuildViewModel(
                filter.ReportDataType,
                (int)ReportTypes.InstaledPrograms,
                filter.DepartmentId,
                filter.SearchFor,
                filter.IsShowParentInventory);

            return this.PartialView("ReportGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ReportsGrid(ReportsSearchFilter filter, int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Reports, ReportFilterMode.DefaultReport.ToString()),
                filter);

            var viewModel = this.BuildViewModel(
                filter.ReportDataType,
                inventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor,
                filter.IsShowParentInventory);

            return this.PartialView("ReportGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult CustomTypeReportGrid(CustomTypeReportsSearchFilter filter, int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Reports, ReportFilterMode.CustomType.ToString()),
                filter);

            var models = this.inventoryService.GetAllConnectedInventory(
                inventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor);

            var viewModel = new ReportViewModel(models.ReportModel, models.InventoryName, filter.IsShowParentInventory);
            return this.PartialView("CustomTypeReportGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoryReportGrid(InventoryReportSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Reports, ReportFilterMode.Inventory.ToString()),
                filter);

            var models = this.inventoryService.GetInventoryCounts(SessionFacade.CurrentCustomer.Id, filter.DepartmentId);

            return this.PartialView("InventoryReportGrid", models);
        }

        [HttpGet]
        public RedirectToRouteResult DeleteInventoryType(int inventoryTypeId)
        {
            this.inventoryService.DeleteInventoryType(inventoryTypeId);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult SearchComputerUsers(string selected)
        {
            var models = this.inventoryService.GetComputerUsers(SessionFacade.CurrentCustomer.Id, selected);

            return this.PartialView("UserSelectDialog", models);
        }

        #region Private

        private ReportViewModel BuildViewModel(
            ReportDataTypes reportDataType,
            int reportType,
            int? departmentId,
            string searchFor,
            bool isShowParentInventory)
        {
            var models = new List<ReportModel>();

            switch (reportDataType)
            {
                case ReportDataTypes.Workstation:
                    models = this.GetComputerReportModels(reportType, departmentId, searchFor);
                    break;
                case ReportDataTypes.Server:
                    models = this.GetServerReportModels(reportType, searchFor);
                    break;
                case ReportDataTypes.All:
                    var computerSoftware = this.GetComputerReportModels(reportType, departmentId, searchFor);
                    var serverSoftware = this.GetServerReportModels(reportType, searchFor);
                    models = computerSoftware.Union(serverSoftware).ToList();
                    break;
            }

            var viewModel = new ReportViewModel(models, ((ReportTypes)reportType).ToString(), isShowParentInventory);

            return viewModel;
        }

        private List<ReportModel> GetComputerReportModels(int reportType, int? departmentId, string searchFor)
        {
            var models = new List<ReportModel>();

            switch ((ReportTypes)reportType)
            {
                case ReportTypes.OperatingSystem:
                    models =
                        this.computerModulesService.GetConnectedToComputerOperatingSystemOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
                    break;
                case ReportTypes.ServicePack:
                    models =
                        this.computerModulesService.GetConnectedToComputerServicePackOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
                    break;
                case ReportTypes.Processor:
                    models =
                        this.computerModulesService.GetConnectedToComputersProcessorsOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
                    break;
                case ReportTypes.NetworkAdapter:
                    models =
                        this.computerModulesService.GetConnectedToComputersNicOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
                    break;
                case ReportTypes.Ram:
                    models =
                        this.computerModulesService.GetConnectedToComputersRamOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
                    break;
                case ReportTypes.Location:
                    models =
                        this.computerModulesService.GetConnectedToComputersLocationOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
                    break;
                case ReportTypes.InstaledPrograms:
                    models =
                        this.computerModulesService.GetComputersInstaledSoftware(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
                    break;
            }

            return models;
        }

        private List<ReportModel> GetServerReportModels(int reportType, string searchFor)
        {
            var models = new List<ReportModel>();

            switch ((ReportTypes)reportType)
            {
                case ReportTypes.OperatingSystem:
                    models =
                        this.computerModulesService.GetConnectedToServerOperatingSystemOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            searchFor);
                    break;
                case ReportTypes.ServicePack:
                    models =
                        this.computerModulesService.GetConnectedToServerServicePackOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            searchFor);
                    break;
                case ReportTypes.Processor:
                    models =
                        this.computerModulesService.GetConnectedToServersProcessorsOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            searchFor);
                    break;
                case ReportTypes.NetworkAdapter:
                    models =
                        this.computerModulesService.GetConnectedToServersNicOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            searchFor);
                    break;
                case ReportTypes.Ram:
                    models =
                        this.computerModulesService.GetConnectedToServersRamOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            searchFor);
                    break;
                case ReportTypes.Location:
                    models =
                        this.computerModulesService.GetConnectedToServersLocationOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            searchFor);
                    break;
                case ReportTypes.InstaledPrograms:
                    models =
                        this.computerModulesService.GetServersInstaledSoftware(
                            SessionFacade.CurrentCustomer.Id,
                            searchFor);
                    break;
            }

            return models;
        }

        private ComputerEditOptions GetWorkstationEditOptions(int customerId)
        {
            var computerModels = this.computerModulesService.GetComputerModels();
            var computerTypes = this.computerModulesService.GetComputerTypes(customerId);
            var operatingSystems = this.computerModulesService.GetOperatingSystems();
            var processors = this.computerModulesService.GetProcessors();
            var rams = this.computerModulesService.GetRams();
            var netAdapters = this.computerModulesService.GetNetAdapters();
            var departments = this.organizationService.GetDepartments(customerId);
            var domains = this.organizationService.GetDepartments(customerId);
            var ous = this.organizationService.GetOrganizationUnits();
            var buildings = this.placeService.GetBuildings(customerId);
            var floors = this.placeService.GetFloors(customerId);
            var rooms = this.placeService.GetRooms(customerId);

            var computerResponse = new ComputerEditOptions(
                computerModels,
                computerTypes,
                operatingSystems,
                processors,
                rams,
                netAdapters,
                departments,
                domains,
                ous,
                buildings,
                floors,
                rooms);

            return computerResponse;
        }

        private ServerEditOptions GetServerEditOptions(int customerId)
        {
            var operatingSystems = this.computerModulesService.GetOperatingSystems();
            var processors = this.computerModulesService.GetProcessors();
            var rams = this.computerModulesService.GetRams();
            var netAdapters = this.computerModulesService.GetNetAdapters();
            var buildings = this.placeService.GetBuildings(customerId);
            var floors = this.placeService.GetFloors(customerId);
            var rooms = this.placeService.GetRooms(customerId);

            return new ServerEditOptions(
                operatingSystems,
                processors,
                rams,
                netAdapters,
                buildings,
                floors,
                rooms);
        }

        private PrinterEditOptions GetPrinterEditOptions(int customerId)
        {
            var departments = this.organizationService.GetDepartments(customerId);
            var buildings = this.placeService.GetBuildings(customerId);
            var floors = this.placeService.GetFloors(customerId);
            var rooms = this.placeService.GetRooms(customerId);

            return new PrinterEditOptions(departments, buildings, floors, rooms);
        }

        private InventoryEditOptions GetInventoryInventoryEditOptions(int customerId)
        {
            var departments = this.organizationService.GetDepartments(customerId);
            var buildings = this.placeService.GetBuildings(customerId);
            var floors = this.placeService.GetFloors(customerId);
            var rooms = this.placeService.GetRooms(customerId);

            return new InventoryEditOptions(departments, buildings, floors, rooms);
        }

        private string CreateFilterId(string tabName, string id)
        {
            return string.Format("{0}{1}", tabName, id);
        }

        #endregion
    }
}
