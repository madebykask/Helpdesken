namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

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

    using PageName = DH.Helpdesk.Web.Enums.PageName;

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

        public ViewResult Index()
        {
            var activeTab = SessionFacade.FindActiveTab(Web.Enums.Inventory.PageName.InventoryIndex) ?? TabName.Inventories;

            var currentModeFilter = SessionFacade.FindPageFilters<InventoriesModeFilter>(TabName.Inventories) ?? InventoriesModeFilter.GetDefault();
            var moduleTypeFilter = SessionFacade.FindPageFilters<ComputerModuleModeFilter>(TabName.MasterData) ?? ComputerModuleModeFilter.GetDefault();

            var inventoryTypes = this.inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id);

            var viewModel = IndexViewModel.BuildViewModel(
                currentModeFilter.CurrentMode,
                inventoryTypes,
                moduleTypeFilter.ModuleType,
                activeTab);

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
            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Workstations));
            var currentFilter = SessionFacade.FindPageFilters<WorkstationsSearchFilter>(CurrentModes.Workstations.ToString()) ?? WorkstationsSearchFilter.CreateDefault();
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
        public PartialViewResult Servers()
        {
            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Servers));
            var currentFilter = SessionFacade.FindPageFilters<ServerSearchFilter>(CurrentModes.Servers.ToString()) ?? ServerSearchFilter.CreateDefault();

            return this.PartialView("Servers", currentFilter);
        }

        [HttpGet]
        public PartialViewResult Printers()
        {
            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Printers));
            var currentFilter = SessionFacade.FindPageFilters<PrinterSearchFilter>(CurrentModes.Printers.ToString()) ?? PrinterSearchFilter.CreateDefault();
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
            var currentFilter = SessionFacade.FindPageFilters<InventorySearchFilter>(inventoryTypeId.ToString(CultureInfo.InvariantCulture)) ?? InventorySearchFilter.CreateDefault(inventoryTypeId);
            var departments = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetInventoryFieldSettingsOverviewForFilter(inventoryTypeId);

            var viewModel = InventorySearchViewModel.BuildViewModel(currentFilter, departments, settings);

            return this.PartialView("Inventories", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            SessionFacade.SavePageFilters(CurrentModes.Workstations.ToString(), filter);
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
            SessionFacade.SavePageFilters(CurrentModes.Servers.ToString(), filter);
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
            SessionFacade.SavePageFilters(CurrentModes.Printers.ToString(), filter);
            var settings = this.inventorySettingsService.GetPrinterFieldSettingsOverview(
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
            var settings = this.inventorySettingsService.GetInventoryFieldSettingsOverview(filter.InventoryTypeId);
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
            var activeTab = SessionFacade.FindActiveTab(Web.Enums.Inventory.PageName.ComputerEdit) ?? TabName.Computer;

            var model = this.inventoryService.GetWorkstation(id);
            var options = this.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var additionalData = this.inventoryService.GetWorkstationEditAdditionalData(
                id,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var computerEditModel = this.computerViewModelBuilder.BuildViewModel(model, options, settings);
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
            var model = this.inventoryService.GetServer(id);
            var options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            var settings = this.inventorySettingsService.GetServerFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var additionalData = this.inventoryService.GetServerEditAdditionalData(
                id,
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

            var serverEditModel = this.serverViewModelBuilder.BuildViewModel(model, options, settings);

            var viewModel = new ServerEditViewModel(
                serverEditModel,
                additionalData.Softwaries,
                additionalData.LogicalDrives,
                additionalData.OperationLogs);

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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewInventory(int inventoryTypeId, string inventoryTypeName)
        {
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
            inventoryViewModel.Name = inventoryTypeName;

            var viewModel = new InventoryEditViewModel(inventoryViewModel, dynamicFieldsModel, typeGroupModels);

            return this.View("EditInventory", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public ViewResult NewInventory(InventoryEditViewModel inventoryEditViewModel)
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
            var models = this.organizationService.GetDepartments(SessionFacade.CurrentCustomer.Id, selected);

            var viewModel = DropDownViewModel.BuildViewModel(models);
            viewModel.AllowEmpty = true;
            viewModel.PropertyName = DropDownName.DepartmentName;
            return this.PartialView("DropDown", viewModel);
        }

        [HttpGet]
        public ActionResult SearchFloorsByBuildingId(int? selected)
        {
            var models = this.placeService.GetFloors(SessionFacade.CurrentCustomer.Id, selected);

            var viewModel = DropDownViewModel.BuildViewModel(models);
            viewModel.AllowEmpty = true;
            viewModel.PropertyName = DropDownName.FloorName;
            return this.PartialView("DropDown", viewModel);
        }

        [HttpGet]
        public ActionResult SearchRoomsByFloorId(int? selected)
        {
            var models = this.placeService.GetRooms(SessionFacade.CurrentCustomer.Id, selected);

            var viewModel = DropDownViewModel.BuildViewModel(models);
            viewModel.AllowEmpty = true;
            viewModel.PropertyName = DropDownName.RoomName;
            return this.PartialView("DropDown", viewModel);
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

        #region Private

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

        #endregion
    }
}
