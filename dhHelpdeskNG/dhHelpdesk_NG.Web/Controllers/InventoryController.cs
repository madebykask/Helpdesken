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
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Printer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
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

    using PageName = DH.Helpdesk.Web.Enums.Inventory.PageName;

    public class InventoryController : UserInteractionController
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

        private readonly IComputerBuilder computerBuilder;

        private readonly IServerBuilder serverBuilder;

        private readonly IPrinterBuilder printerBuilder;

        private readonly IInventoryModelBuilder inventoryModelBuilder;

        private readonly IInventoryValueBuilder inventoryValueBuilder;

        private readonly IExcelFileComposer excelFileComposer;

        private readonly IExportFileNameFormatter exportFileNameFormatter;

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
            IDynamicsFieldsModelBuilder dynamicsFieldsModelBuilder,
            IComputerBuilder computerBuilder,
            IServerBuilder serverBuilder,
            IPrinterBuilder printerBuilder,
            IInventoryModelBuilder inventoryModelBuilder,
            IInventoryValueBuilder inventoryValueBuilder,
            IExcelFileComposer excelFileComposer,
            IExportFileNameFormatter exportFileNameFormatter)
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
            this.computerBuilder = computerBuilder;
            this.serverBuilder = serverBuilder;
            this.printerBuilder = printerBuilder;
            this.inventoryModelBuilder = inventoryModelBuilder;
            this.inventoryValueBuilder = inventoryValueBuilder;
            this.excelFileComposer = excelFileComposer;
            this.exportFileNameFormatter = exportFileNameFormatter;
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")] // todo
        public ViewResult Index()
        {
            var activeTab = SessionFacade.FindActiveTab(PageName.InventoryIndex) ?? TabName.Inventories;

            var currentModeFilter = SessionFacade.FindPageFilters<InventoriesModeFilter>(TabName.Inventories)
                                    ?? InventoriesModeFilter.GetDefault();
            var reportTypeFilter = SessionFacade.FindPageFilters<ReportFilter>(TabName.Reports)
                                   ?? ReportFilter.GetDefault();
            var moduleTypeFilter = SessionFacade.FindPageFilters<ComputerModuleModeFilter>(TabName.MasterData)
                                   ?? ComputerModuleModeFilter.GetDefault();

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
            var settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsOverviewForFilter(
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
            var settings =
                this.inventorySettingsService.GetPrinterFieldSettingsOverviewForFilter(
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

            var viewModel = InventorySearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                settings,
                inventoryTypeId);

            return this.PartialView("Inventories", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Workstation.ToString()),
                filter);

            var viewModel = this.CreateInventoryGridModel(filter);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ServersGrid(ServerSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Server.ToString()),
                filter);

            var viewModel = this.CreateInventoryGridModel(filter);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult PrintersGrid(PrinterSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Printer.ToString()),
                filter);

            var viewModel = this.CreateInventoryGridModel(filter);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoriesGrid(InventorySearchFilter filter, int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.CustomType.ToString()),
                filter);

            var viewModel = this.CreateInventoryGridModel(filter, inventoryTypeId);

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
                    return this.RedirectToAction("EditInventory", new { id, inventoryTypeId = currentMode });
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
            var settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            var softwares = this.computerModulesService.GetComputerSoftware(id);
            var drives = this.computerModulesService.GetComputerLogicalDrive(id);
            var logs = this.inventoryService.GetWorkstationLogOverviews(id);
            var computerEditModel = this.computerViewModelBuilder.BuildViewModel(model, options, settings);

            var viewModel = new ComputerEditViewModel(computerEditModel, softwares, drives, logs, activeTab);

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
                                  ? this.inventoryService.GetNotConnectedInventory(selected.Value, computerId)
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
        public RedirectToRouteResult EditWorkstation(ComputerViewModel computerViewModel)
        {
            var businessModel = this.computerBuilder.BuildForUpdate(computerViewModel, this.OperationContext);
            this.inventoryService.UpdateWorkstation(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditServer(int id)
        {
            var model = this.inventoryService.GetServer(id);
            var options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings =
                this.inventorySettingsService.GetServerFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            var softwares = this.computerModulesService.GetServerSoftware(id);
            var drives = this.computerModulesService.GetServerLogicalDrive(id);
            var logs = this.inventoryService.GetOperationServerLogOverviews(id, SessionFacade.CurrentCustomer.Id);

            var serverEditModel = this.serverViewModelBuilder.BuildViewModel(model, options, settings);

            var viewModel = new ServerEditViewModel(serverEditModel, softwares, drives, logs);

            return this.View("EditServer", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult EditServer(ServerViewModel serverViewModel)
        {
            var businessModel = this.serverBuilder.BuildForUpdate(serverViewModel, this.OperationContext);
            this.inventoryService.UpdateServer(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditPrinter(int id)
        {
            var model = this.inventoryService.GetPrinter(id);
            var options = this.GetPrinterEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings =
                this.inventorySettingsService.GetPrinterFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            var printerEditModel = this.printerViewModelBuilder.BuildViewModel(model, options, settings);

            return this.View("EditPrinter", printerEditModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult EditPrinter(PrinterViewModel printerViewModel)
        {
            var businessModel = this.printerBuilder.BuildForUpdate(printerViewModel, this.OperationContext);
            this.inventoryService.UpdatePrinter(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
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
        public RedirectToRouteResult EditInventory(
            InventoryViewModel inventoryViewModel,
            List<DynamicFieldModel> dynamicFieldModels)
        {
            var businessModel = this.inventoryModelBuilder.BuildForUpdate(inventoryViewModel, OperationContext);
            var dynamicBusinessModels = this.inventoryValueBuilder.BuildForWrite(
                inventoryViewModel.Id,
                dynamicFieldModels);
            this.inventoryService.UpdateInventory(
                businessModel,
                dynamicBusinessModels,
                inventoryViewModel.InventoryTypeId);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult DeleteWorkstation(int id)
        {
            this.inventoryService.DeleteWorkstation(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult DeleteServer(int id)
        {
            this.inventoryService.DeleteServer(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult DeletePrinter(int id)
        {
            this.inventoryService.DeletePrinter(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public RedirectToRouteResult DeleteInventory(int id)
        {
            this.inventoryService.DeleteInventory(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewWorkstation()
        {
            var options = this.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
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
        public RedirectToRouteResult NewWorkstation(ComputerViewModel computerViewModel)
        {
            var businessModel = this.computerBuilder.BuildForAdd(computerViewModel, this.OperationContext);
            this.inventoryService.AddWorkstation(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewServer()
        {
            var options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings =
                this.inventorySettingsService.GetServerFieldSettingsForModelEdit(
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
        public RedirectToRouteResult NewServer(ServerViewModel serverViewModel)
        {
            var businessModel = this.serverBuilder.BuildForAdd(serverViewModel, this.OperationContext);
            this.inventoryService.AddServer(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewPrinter()
        {
            var options = this.GetPrinterEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings =
                this.inventorySettingsService.GetPrinterFieldSettingsForModelEdit(
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
        public RedirectToRouteResult NewPrinter(PrinterViewModel printerViewModel)
        {
            var businessModel = this.printerBuilder.BuildForAdd(printerViewModel, this.OperationContext);
            this.inventoryService.AddPrinter(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
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

            return this.View("NewInventory", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewInventory(
            InventoryViewModel inventoryViewModel,
            List<DynamicFieldModel> dynamicFieldModels)
        {
            var businessModel = this.inventoryModelBuilder.BuildForAdd(inventoryViewModel, OperationContext);
            this.inventoryService.AddInventory(businessModel);
            var dynamicBusinessModels = this.inventoryValueBuilder.BuildForWrite(businessModel.Id, dynamicFieldModels);
            this.inventoryService.AddDynamicFieldsValuesInventory(dynamicBusinessModels);

            return this.RedirectToAction("Index");
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

            var viewModel = InstaledProgramsReportSearchViewModel.BuildViewModel(currentFilter, departments);

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

            var viewModel = CustomTypeReportSearchViewModel.BuildViewModel(currentFilter, departments, inventoryTypeId);

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

            var viewModel = InventoryReportSearchViewModel.BuildViewModel(currentFilter, departments);

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

            ReportViewModel viewModel = this.BuildViewModel(
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

            ReportViewModel viewModel = this.BuildViewModel(
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

            ReportModelWithInventoryType models = this.inventoryService.GetAllConnectedInventory(
                inventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor);

            List<ReportModelWrapper> wrapReportModels = ReportModelWrapper.Wrap(
                models.ReportModel,
                ReportModelWrapper.ReportOwnerTypes.Workstation);

            var viewModel = new ReportViewModel(wrapReportModels, models.InventoryName, filter.IsShowParentInventory);

            return this.PartialView("CustomTypeReportGrid", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoryReportGrid(InventoryReportSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Reports, ReportFilterMode.Inventory.ToString()),
                filter);

            List<InventoryReportModel> models = this.inventoryService.GetInventoryCounts(SessionFacade.CurrentCustomer.Id, filter.DepartmentId);

            return this.PartialView("InventoryReportGrid", models);
        }

        [HttpGet]
        public RedirectToRouteResult DeleteInventoryType(int inventoryTypeId)
        {
            this.inventoryService.DeleteInventoryType(inventoryTypeId);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult SearchComputerShortInfo(int computerId)
        {
            var model = this.inventoryService.GetWorkstationShortInfo(computerId);
            var settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsForShortInfo(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            var softwares = this.computerModulesService.GetComputerSoftware(computerId);
            var drives = this.computerModulesService.GetComputerLogicalDrive(computerId);
            var logs = this.inventoryService.GetWorkstationLogOverviews(computerId);

            var viewModel = new ComputerModalViewModel(model, settings, softwares, drives, logs);

            return this.PartialView("ComputerShortInfoDialog", viewModel);
        }

        [HttpGet]
        public PartialViewResult SearchComputerUsers(string selected)
        {
            var models = this.inventoryService.GetComputerUsers(SessionFacade.CurrentCustomer.Id, selected);

            return this.PartialView("UserSelectDialog", models);
        }

        [HttpGet]
        public PartialViewResult SearchComputerUserHistory(int computerId)
        {
            var models = this.inventoryService.GetComputerUserHistory(computerId);

            return this.PartialView("UserHistoryDialog", models);
        }

        [HttpPost]
        public void EditComputerInfo(int id, string info)
        {
            this.inventoryService.UpdateWorkstationInfo(id, info);
        }

        [HttpGet]
        public FileContentResult ExportReportToExcelFile(int reportType)
        {
            switch ((ReportTypes)reportType)
            {
                case ReportTypes.OperatingSystem:
                case ReportTypes.ServicePack:
                case ReportTypes.Processor:
                case ReportTypes.Ram:
                case ReportTypes.NetworkAdapter:
                case ReportTypes.Location:
                    return this.CreateModuleReport(reportType);
                case ReportTypes.InstaledPrograms:
                    return this.CreateInstaledProgramReport();
                case ReportTypes.Inventory:
                    return this.CreateInventoryReport();
                default:
                    return this.CreateCustomTypeReport(reportType);
            }
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile(int currentMode)
        {
            InventoryGridModel gridModel;
            string workSheetName;

            switch ((CurrentModes)currentMode)
            {
                case CurrentModes.Workstations:
                    var workstationFilter =
                        SessionFacade.FindPageFilters<WorkstationsSearchFilter>(
                            this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Workstation.ToString()))
                        ?? WorkstationsSearchFilter.CreateDefault();
                    gridModel = this.CreateInventoryGridModel(workstationFilter);
                    workSheetName = CurrentModes.Workstations.ToString();
                    break;
                case CurrentModes.Servers:
                    var serverFilter =
                        SessionFacade.FindPageFilters<ServerSearchFilter>(
                            this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Server.ToString()))
                        ?? ServerSearchFilter.CreateDefault();
                    gridModel = this.CreateInventoryGridModel(serverFilter);
                    workSheetName = CurrentModes.Servers.ToString();
                    break;
                case CurrentModes.Printers:
                    var printerFilter =
                        SessionFacade.FindPageFilters<PrinterSearchFilter>(
                            this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Printer.ToString()))
                        ?? PrinterSearchFilter.CreateDefault();
                    gridModel = this.CreateInventoryGridModel(printerFilter);
                    workSheetName = CurrentModes.Printers.ToString();
                    break;
                default:
                    var inventoryFilter =
                        SessionFacade.FindPageFilters<InventorySearchFilter>(
                            this.CreateFilterId(TabName.Inventories, InventoryFilterMode.CustomType.ToString()))
                        ?? InventorySearchFilter.CreateDefault(currentMode);
                    gridModel = this.CreateInventoryGridModel(inventoryFilter, currentMode);

                    // todo move into invetory overview query
                    var inventoryType = this.inventoryService.GetInventoryType(currentMode);
                    workSheetName = inventoryType.Name;
                    break;
            }

            var file = this.CreateExcelReport(workSheetName, gridModel.Headers, gridModel.Inventories);

            return file;
        }

        #region Private

        private FileContentResult CreateInstaledProgramReport()
        {
            InstaledProgramsReportSearchFilter filter =
                SessionFacade.FindPageFilters<InstaledProgramsReportSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.InstaledPrograms.ToString()))
                ?? InstaledProgramsReportSearchFilter.CreateDefault();

            if (filter.IsShowMissingParentInventory)
            {
                return null;
            }

            ReportViewModel viewModel = this.BuildViewModel(
                filter.ReportDataType,
                (int)ReportTypes.InstaledPrograms,
                filter.DepartmentId,
                filter.SearchFor,
                filter.IsShowParentInventory);

            return this.CreateReportGridReport(viewModel);
        }

        private FileContentResult CreateModuleReport(int inventoryTypeId)
        {
            ReportsSearchFilter filter =
                SessionFacade.FindPageFilters<ReportsSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.DefaultReport.ToString()))
                ?? ReportsSearchFilter.CreateDefault();

            ReportViewModel viewModel = this.BuildViewModel(
                filter.ReportDataType,
                inventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor,
                filter.IsShowParentInventory);

            return this.CreateReportGridReport(viewModel);
        }

        private FileContentResult CreateCustomTypeReport(int inventoryTypeId)
        {
            CustomTypeReportsSearchFilter filter =
                SessionFacade.FindPageFilters<CustomTypeReportsSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.CustomType.ToString()))
                ?? CustomTypeReportsSearchFilter.CreateDefault();

            ReportModelWithInventoryType models = this.inventoryService.GetAllConnectedInventory(
                inventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor);

            return this.CreateCustomTypeReport(models, filter.IsShowParentInventory);
        }

        private FileContentResult CreateInventoryReport()
        {
            var filter =
                SessionFacade.FindPageFilters<InventoryReportSearchFilter>(
                    this.CreateFilterId(TabName.Reports, ReportFilterMode.Inventory.ToString()))
                ?? InventoryReportSearchFilter.CreateDefault();

            List<InventoryReportModel> models =
                this.inventoryService.GetInventoryCounts(SessionFacade.CurrentCustomer.Id, filter.DepartmentId);

            return this.CreateInventoryReport(models);
        }

        private FileContentResult CreateReportGridReport(ReportViewModel viewModel)
        {
            const string NameCellFieldName = "Name";
            const string CountCellFieldName = "Count";

            var headers = new List<ExcelTableHeader>
                              {
                                  new ExcelTableHeader(viewModel.Header, NameCellFieldName),
                                  new ExcelTableHeader(Translation.Get("Antal"), CountCellFieldName)
                              };

            var source = new List<BusinessItem>();

            IEnumerable<IGrouping<string, ReportModelWrapper>> items =
                viewModel.ReportModel.OrderBy(x => x.ReportModel.Item)
                    .ThenBy(x => x.ReportModel.Owner)
                    .GroupBy(x => x.ReportModel.Item);

            foreach (var item in items)
            {
                var nameCell = new BusinessItemField(NameCellFieldName, new StringDisplayValue(item.Key))
                {
                    IsBold =
                        viewModel
                        .IsGrouped
                };
                var countCell = new BusinessItemField(CountCellFieldName, new IntegerDisplayValue(item.Count()))
                {
                    IsBold =
                        viewModel
                        .IsGrouped
                };

                var cells = new List<BusinessItemField> { nameCell, countCell };
                var businessItem = new BusinessItem(cells);
                source.Add(businessItem);

                if (viewModel.IsGrouped)
                {
                    source.AddRange(
                        from owner in item.Where(x => x.ReportModel.Owner != null)
                        select new BusinessItemField(NameCellFieldName, new StringDisplayValue(owner.ReportModel.Owner))
                            into ownerCell
                            select new List<BusinessItemField> { ownerCell }
                                into ownerCells
                                select new BusinessItem(ownerCells));
                }
            }

            var file = this.CreateExcelReport(viewModel.Header, headers, source);

            return file;
        }

        private FileContentResult CreateCustomTypeReport(ReportModelWithInventoryType viewModel, bool isGrouped)
        {
            const string NameCellFieldName = "Name";

            var headers = new List<ExcelTableHeader>
                              {
                                  new ExcelTableHeader(viewModel.InventoryName, NameCellFieldName)
                              };

            var source = new List<BusinessItem>();

            IEnumerable<IGrouping<string, ReportModel>> items =
                viewModel.ReportModel.OrderBy(x => x.Item).ThenBy(x => x.Owner).GroupBy(x => x.Item);

            foreach (var item in items)
            {
                var nameCell = new BusinessItemField(NameCellFieldName, new StringDisplayValue(item.Key))
                                   {
                                       IsBold =
                                           isGrouped
                                   };

                var cells = new List<BusinessItemField> { nameCell };
                var businessItem = new BusinessItem(cells);
                source.Add(businessItem);

                if (isGrouped)
                {
                    source.AddRange(
                        from owner in item.Where(x => x.Owner != null)
                        select new BusinessItemField(NameCellFieldName, new StringDisplayValue(owner.Owner))
                            into ownerCell
                            select new List<BusinessItemField> { ownerCell }
                                into ownerCells
                                select new BusinessItem(ownerCells));
                }
            }

            var file = this.CreateExcelReport(viewModel.InventoryName, headers, source);

            return file;
        }

        private FileContentResult CreateInventoryReport(List<InventoryReportModel> models)
        {
            const string NameCellFieldName = "Name";
            const string CountCellFieldName = "Count";

            var headers = new List<ExcelTableHeader>
                              {
                                  new ExcelTableHeader(Translation.Get("Inventarie"), NameCellFieldName),
                                  new ExcelTableHeader(Translation.Get("Antal"), CountCellFieldName)
                              };

            var source = (from model in models
                          let nameCell = new BusinessItemField(NameCellFieldName, new StringDisplayValue(model.InventoryName))
                          let countCell = new BusinessItemField(CountCellFieldName, new IntegerDisplayValue(model.Count))
                          select new List<BusinessItemField> { nameCell, countCell }
                              into cells
                              select new BusinessItem(cells)).ToList();

            var file = this.CreateExcelReport(Translation.Get("Inventarie"), headers, source);

            return file;
        }

        private ReportViewModel BuildViewModel(
            ReportDataTypes reportDataType,
            int reportType,
            int? departmentId,
            string searchFor,
            bool isShowParentInventory)
        {
            var wrapModels = new List<ReportModelWrapper>();

            switch (reportDataType)
            {
                case ReportDataTypes.Workstation:
                    wrapModels = this.CreateWorkstationReportModelWrappers(reportType, departmentId, searchFor);
                    break;
                case ReportDataTypes.Server:
                    wrapModels = this.CreateServerReportModelWrappers(reportType, searchFor);
                    break;
                case ReportDataTypes.All:
                    var computerSoftware = this.CreateWorkstationReportModelWrappers(
                        reportType,
                        departmentId,
                        searchFor);
                    var serverSoftware = this.CreateServerReportModelWrappers(reportType, searchFor);
                    wrapModels = computerSoftware.Union(serverSoftware).ToList();
                    break;
            }

            var viewModel = new ReportViewModel(wrapModels, ((ReportTypes)reportType).ToString(), isShowParentInventory);

            return viewModel;
        }

        private List<ReportModelWrapper> CreateWorkstationReportModelWrappers(
            int reportType,
            int? departmentId,
            string searchFor)
        {
            var computerModels = this.GetComputerReportModels(reportType, departmentId, searchFor);
            var wrapModels = ReportModelWrapper.Wrap(computerModels, ReportModelWrapper.ReportOwnerTypes.Workstation);
            return wrapModels;
        }

        private List<ReportModelWrapper> CreateServerReportModelWrappers(int reportType, string searchFor)
        {
            var serverModels = this.GetServerReportModels(reportType, searchFor);
            var wrapModels = ReportModelWrapper.Wrap(serverModels, ReportModelWrapper.ReportOwnerTypes.Server);
            return wrapModels;
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
                    models = this.computerModulesService.GetComputersInstaledSoftware(
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
                    models = this.computerModulesService.GetServersInstaledSoftware(
                        SessionFacade.CurrentCustomer.Id,
                        searchFor);
                    break;
            }

            return models;
        }

        private ComputerEditOptions GetWorkstationEditOptions(int customerId)
        {
            List<ItemOverview> computerModels =
                this.computerModulesService.GetComputerModels().OrderBy(x => x.Name).ToList();
            List<ItemOverview> computerTypes =
                this.computerModulesService.GetComputerTypes(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> operatingSystems =
                this.computerModulesService.GetOperatingSystems().OrderBy(x => x.Name).ToList();
            List<ItemOverview> processors = this.computerModulesService.GetProcessors().OrderBy(x => x.Name).ToList();
            List<ItemOverview> rams = this.computerModulesService.GetRams().OrderBy(x => x.Name).ToList();
            List<ItemOverview> netAdapters = this.computerModulesService.GetNetAdapters().OrderBy(x => x.Name).ToList();
            List<ItemOverview> departments =
                this.organizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> domains =
                this.organizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> ous = this.organizationService.GetOrganizationUnits().OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.placeService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.placeService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.placeService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

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
            List<ItemOverview> operatingSystems =
                this.computerModulesService.GetOperatingSystems().OrderBy(x => x.Name).ToList();
            List<ItemOverview> processors = this.computerModulesService.GetProcessors().OrderBy(x => x.Name).ToList();
            List<ItemOverview> rams = this.computerModulesService.GetRams().OrderBy(x => x.Name).ToList();
            List<ItemOverview> netAdapters = this.computerModulesService.GetNetAdapters().OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.placeService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.placeService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.placeService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

            return new ServerEditOptions(operatingSystems, processors, rams, netAdapters, buildings, floors, rooms);
        }

        private PrinterEditOptions GetPrinterEditOptions(int customerId)
        {
            List<ItemOverview> departments =
                this.organizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.placeService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.placeService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.placeService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

            return new PrinterEditOptions(departments, buildings, floors, rooms);
        }

        private InventoryEditOptions GetInventoryInventoryEditOptions(int customerId)
        {
            List<ItemOverview> departments =
                this.organizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.placeService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.placeService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.placeService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

            return new InventoryEditOptions(departments, buildings, floors, rooms);
        }

        private string CreateFilterId(string tabName, string id)
        {
            return string.Format("{0}{1}", tabName, id);
        }

        private InventoryGridModel CreateInventoryGridModel(WorkstationsSearchFilter filter)
        {
            ComputerFieldsSettingsOverview settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsOverview(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            List<ComputerOverview> models =
                this.inventoryService.GetWorkstations(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }

        private InventoryGridModel CreateInventoryGridModel(ServerSearchFilter filter)
        {
            ServerFieldsSettingsOverview settings = this.inventorySettingsService.GetServerFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            List<ServerOverview> models = this.inventoryService.GetServers(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }

        private InventoryGridModel CreateInventoryGridModel(PrinterSearchFilter filter)
        {
            PrinterFieldsSettingsOverview settings = this.inventorySettingsService.GetPrinterFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            List<PrinterOverview> models = this.inventoryService.GetPrinters(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }

        private InventoryGridModel CreateInventoryGridModel(InventorySearchFilter filter, int inventoryTypeId)
        {
            InventoryFieldSettingsOverviewResponse settings = this.inventorySettingsService.GetInventoryFieldSettingsOverview(inventoryTypeId);
            InventoriesOverviewResponse models = this.inventoryService.GetInventories(filter.CreateRequest(inventoryTypeId));

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(models, settings, inventoryTypeId, filter.SortField);
            return viewModel;
        }

        private FileContentResult CreateExcelReport(
            string worksheetName,
            IEnumerable<ITableHeader> headers,
            IEnumerable<IRow<ICell>> rows)
        {
            const string Name = "Inventory";

            var content = this.excelFileComposer.Compose(headers, rows, worksheetName);

            var fileName = this.exportFileNameFormatter.Format(Name, "xlsx");
            return this.File(content, MimeType.ExcelFile, fileName);
        }

        #endregion
    }
}
