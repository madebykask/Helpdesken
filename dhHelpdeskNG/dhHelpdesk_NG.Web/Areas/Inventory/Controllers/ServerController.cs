using DH.Helpdesk.Common.Constants;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ServerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server;
    using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;
    using DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    public class ServerController : InventoryBaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IInventorySettingsService inventorySettingsService;

        private readonly IComputerModulesService computerModulesService;

        private readonly IServerViewModelBuilder serverViewModelBuilder;

        private readonly IServerBuilder serverBuilder;

        private readonly IUserPermissionsChecker _userPermissionsChecker;

        public ServerController(
            IMasterDataService masterDataService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService,
            IComputerModulesService computerModulesService,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IServerViewModelBuilder serverViewModelBuilder,
            IServerBuilder serverBuilder,
            IExcelFileComposer excelFileComposer,
            IUserPermissionsChecker userPermissionsChecker,
            IExportFileNameFormatter exportFileNameFormatter)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this.inventoryService = inventoryService;
            this.inventorySettingsService = inventorySettingsService;
            this.computerModulesService = computerModulesService;
            this.serverViewModelBuilder = serverViewModelBuilder;
            this.serverBuilder = serverBuilder;
            this._userPermissionsChecker = userPermissionsChecker;
        }

        [HttpGet]
        public ViewResult Index()
        {
            List<ItemOverview> inventoryTypes = this.inventoryService.GetInventoryTypes(
                SessionFacade.CurrentCustomer.Id);

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Servers));
            ServerSearchFilter currentFilter =
                SessionFacade.FindPageFilters<ServerSearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Server.ToString()))
                ?? ServerSearchFilter.CreateDefault();

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            var viewModel = new ServerSearchViewModel((int)CurrentModes.Servers, inventoryTypes, currentFilter);
            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ServersGrid(ServerSearchFilter filter)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Server.ToString()),
                filter);
            filter.RecordsCount = SearchFilter.RecordsOnPage;

            var viewModel = this.CreateInventoryGridModel(filter);

            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpGet]
        public ViewResult New()
        {
            ServerEditOptions options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            ServerFieldsSettingsForModelEdit settings =
                this.inventorySettingsService.GetServerFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            ServerViewModel viewModel = this.serverViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(ServerViewModel serverViewModel)
        {
            ServerForInsert businessModel = this.serverBuilder.BuildForAdd(serverViewModel, this.OperationContext);
            this.inventoryService.AddServer(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int id, bool dialog = false)
        {
            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);
            var readOnly = !userHasInventoryAdminPermission && dialog;

            ServerForRead model = this.inventoryService.GetServer(id);
            ServerEditOptions options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            ServerFieldsSettingsForModelEdit settings = inventorySettingsService.GetServerFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId, readOnly);

            ServerViewModel serverEditModel = this.serverViewModelBuilder.BuildViewModel(model, options, settings);
            serverEditModel.IsForDialog = dialog;
            var viewModel = new ServerEditViewModel(id, serverEditModel)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission,
                IsForDialog = dialog
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(ServerViewModel serverViewModel)
        {
            ServerForUpdate businessModel = this.serverBuilder.BuildForUpdate(serverViewModel, this.OperationContext);
            this.inventoryService.UpdateServer(businessModel, this.OperationContext);
            if (serverViewModel.IsForDialog)
            {
                return RedirectToAction("Edit", new { id = serverViewModel.Id, dialog = serverViewModel.IsForDialog });
            }
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.inventoryService.DeleteServer(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile()
        {
            ServerSearchFilter serverFilter =
                SessionFacade.FindPageFilters<ServerSearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.Server.ToString()))
                ?? ServerSearchFilter.CreateDefault();
            InventoryGridModel gridModel = this.CreateInventoryGridModel(serverFilter);
            string workSheetName = CurrentModes.Servers.ToString();

            FileContentResult file = this.CreateExcelReport(workSheetName, gridModel.Headers, gridModel.Inventories);

            return file;
        }

        [HttpGet]
        public ViewResult Storage(int serverId, bool dialog = false)
        {
            List<LogicalDriveOverview> models = this.computerModulesService.GetServerLogicalDrive(serverId);

            var viewModel = new StorageViewModel(serverId, models) { IsForDialog = dialog };

            return this.View(viewModel);
        }

        [HttpGet]
        public ViewResult Software(int serverId, bool dialog = false)
        {
            List<SoftwareOverview> models = this.computerModulesService.GetServerSoftware(serverId);

            var viewModel = new SoftwareViewModel(serverId, models) { IsForDialog = dialog };

            return this.View(viewModel);
        }

        [HttpGet]
        public ViewResult HotFixes(int serverId, bool dialog = false)
        {
            List<SoftwareOverview> models = this.computerModulesService.GetServerSoftware(serverId);

            var viewModel = new HotfixViewModel(serverId, models) { IsForDialog = dialog };

            return this.View(viewModel);
        }

        [HttpGet]
        public ViewResult Logs(int serverId, bool dialog = false)
        {
            List<OperationServerLogOverview> models = this.inventoryService.GetOperationServerLogOverviews(serverId, SessionFacade.CurrentCustomer.Id);

            var viewModel = new LogsViewModel(serverId, models) {IsForDialog = dialog};

            return this.View(viewModel);
        }

        private ServerEditOptions GetServerEditOptions(int customerId)
        {
            List<ItemOverview> operatingSystems =
                this.computerModulesService.GetOperatingSystems().OrderBy(x => x.Name).ToList();
            List<ItemOverview> processors = this.computerModulesService.GetProcessors().OrderBy(x => x.Name).ToList();
            List<ItemOverview> rams = this.computerModulesService.GetRams().OrderBy(x => x.Name).ToList();
            List<ItemOverview> netAdapters = this.computerModulesService.GetNetAdapters().OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.PlaceService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.PlaceService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.PlaceService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

            return new ServerEditOptions(operatingSystems, processors, rams, netAdapters, buildings, floors, rooms);
        }

        private InventoryGridModel CreateInventoryGridModel(ServerSearchFilter filter)
        {
            ServerFieldsSettingsOverview settings = this.inventorySettingsService.GetServerFieldSettingsOverview(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            var models = this.inventoryService.GetServers(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }
    }
}