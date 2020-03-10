namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.InventoryFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory;
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

    public class CustomInventoryController : InventoryBaseController
    {
        private readonly IInventorySettingsService _inventorySettingsService;
        private readonly IInventoryService _inventoryService;
        private readonly IInventoryViewModelBuilder _inventoryViewModelBuilder;
        private readonly IDynamicsFieldsModelBuilder _dynamicsFieldsModelBuilder;
        private readonly IInventoryModelBuilder _inventoryModelBuilder;
        private readonly IInventoryValueBuilder _inventoryValueBuilder;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly IComputerModulesService _computerModulesService;

        public CustomInventoryController(
            IMasterDataService masterDataService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IInventorySettingsService inventorySettingsService,
            IInventoryService inventoryService,
            IInventoryViewModelBuilder inventoryViewModelBuilder,
            IDynamicsFieldsModelBuilder dynamicsFieldsModelBuilder,
            IInventoryModelBuilder inventoryModelBuilder,
            IInventoryValueBuilder inventoryValueBuilder,
            IUserPermissionsChecker userPermissionsChecker,
            IComputerModulesService computerModulesService)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this._inventorySettingsService = inventorySettingsService;
            this._inventoryService = inventoryService;
            this._inventoryViewModelBuilder = inventoryViewModelBuilder;
            this._dynamicsFieldsModelBuilder = dynamicsFieldsModelBuilder;
            this._inventoryModelBuilder = inventoryModelBuilder;
            this._inventoryValueBuilder = inventoryValueBuilder;
            this._userPermissionsChecker = userPermissionsChecker;
            _computerModulesService = computerModulesService;
        }

        [HttpGet]
        public ViewResult Index(int inventoryTypeId)
        {
            var inventoryTypes =
                this._inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id, true, CreateInventoryTypeSeparatorItem());

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter(inventoryTypeId));
            var currentFilter =
                SessionFacade.FindPageFilters<InventorySearchFilter>(InventorySearchFilter.CreateFilterId())
                ?? InventorySearchFilter.CreateDefault(inventoryTypeId);

            var departments = OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            var fieldsSettings = _inventorySettingsService.GetInventoryFieldSettingsOverviewForFilter(inventoryTypeId);

            var userHasInventoryAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);

            var viewModel = InventorySearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                fieldsSettings,
                inventoryTypeId,
                inventoryTypeId,
                inventoryTypes);

            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoriesGrid(InventorySearchFilter filter, int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(InventorySearchFilter.CreateFilterId(), filter);

            var viewModel = CreateInventoryGridModel(filter, inventoryTypeId);

            return this.PartialView("CustomInventoryGrid", viewModel);
        }

        [HttpGet]
        public ViewResult New(int inventoryTypeId)
        {
            InventoryType inventoryType = this._inventoryService.GetInventoryType(inventoryTypeId);
            InventoryFieldSettingsForModelEditResponse settings =
                this._inventorySettingsService.GetInventoryFieldSettingsForModelEdit(inventoryTypeId);
            InventoryEditOptions options = this.GetInventoryInventoryEditOptions(SessionFacade.CurrentCustomer.Id, inventoryTypeId);
            List<TypeGroupModel> typeGroupModels = this._inventoryService.GetTypeGroupModels(inventoryTypeId);

            InventoryViewModel inventoryViewModel = this._inventoryViewModelBuilder.BuildViewModel(
                options,
                settings.InventoryFieldSettingsForModelEdit,
                inventoryTypeId,
                SessionFacade.CurrentCustomer.Id);

            List<DynamicFieldModel> dynamicFieldsModel =
                this._dynamicsFieldsModelBuilder.BuildViewModel(settings.InventoryDynamicFieldSettingForModelEditData);

            var viewModel = new InventoryEditViewModel(0, inventoryViewModel, dynamicFieldsModel, typeGroupModels);
            viewModel.InventoryName = inventoryType.Name;

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(
            InventoryViewModel inventoryViewModel,
            List<DynamicFieldModel> allModels)
        {
            InventoryForInsert businessModel = this._inventoryModelBuilder.BuildForAdd(
                inventoryViewModel,
                OperationContext);
            this._inventoryService.AddInventory(businessModel);
            List<InventoryValueForWrite> dynamicBusinessModels =
                this._inventoryValueBuilder.BuildForWrite(businessModel.Id, allModels);
            this._inventoryService.AddDynamicFieldsValuesInventory(dynamicBusinessModels);

            return this.RedirectToAction("Index", new { inventoryTypeId = inventoryViewModel.InventoryTypeId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int inventoryTypeId, int id)
        {
            this._inventoryService.DeleteInventory(id);

            return this.RedirectToAction("Index", new { inventoryTypeId });
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile(int inventoryTypeId)
        {
            var inventoryFilter =
                SessionFacade.FindPageFilters<InventorySearchFilter>(InventorySearchFilter.CreateFilterId()) ?? 
                InventorySearchFilter.CreateDefault(inventoryTypeId);

            var gridModel = CreateInventoryGridModel(inventoryFilter, inventoryTypeId, true);

            // todo move into invetory overview query
            var inventoryType = _inventoryService.GetInventoryType(inventoryTypeId);
            
            var file = CreateExcelReport(inventoryType.Name, gridModel.Headers, gridModel.Inventories);
            return file;
        }

        [HttpGet]
        public ViewResult Edit(int id, int inventoryTypeId, bool dialog = false)
        {
            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);
            var readOnly = !userHasInventoryAdminPermission && dialog;

            var model = this._inventoryService.GetInventory(id);
            var settings = this._inventorySettingsService.GetInventoryFieldSettingsForModelEdit(inventoryTypeId, readOnly);
            var options = this.GetInventoryInventoryEditOptions(SessionFacade.CurrentCustomer.Id, inventoryTypeId);
            var typeGroupModels = this._inventoryService.GetTypeGroupModels(inventoryTypeId);

            var inventoryComputerTypes = _computerModulesService.GetComputerTypes(SessionFacade.CurrentCustomer.Id, inventoryTypeId);
            ViewBag.HasType = inventoryComputerTypes.Any();

            var inventoryViewModel = this._inventoryViewModelBuilder.BuildViewModel(model.Inventory, options, settings.InventoryFieldSettingsForModelEdit);
            var dynamicFieldsModel = this._dynamicsFieldsModelBuilder.BuildViewModel(model.DynamicData, settings.InventoryDynamicFieldSettingForModelEditData, id);
            inventoryViewModel.IsForDialog = dialog;

            var viewModel = new InventoryEditViewModel(id, inventoryViewModel, dynamicFieldsModel, typeGroupModels)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission,
                IsForDialog = dialog,
                InventoryName = model.Inventory.InventoryTypeName,
                InventoryTypeId = inventoryTypeId
            };

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(InventoryViewModel inventoryViewModel, List<DynamicFieldModel> allModels)
        {
            InventoryForUpdate businessModel = this._inventoryModelBuilder.BuildForUpdate(inventoryViewModel, OperationContext);
            List<InventoryValueForWrite> dynamicBusinessModels = this._inventoryValueBuilder.BuildForWrite(inventoryViewModel.Id, allModels);
            this._inventoryService.UpdateInventory(businessModel, dynamicBusinessModels, inventoryViewModel.InventoryTypeId);

            if (inventoryViewModel.IsForDialog)
            {
                return RedirectToAction("Edit", new { id = inventoryViewModel.Id, inventoryTypeId = inventoryViewModel.InventoryTypeId, dialog = inventoryViewModel.IsForDialog });
            }

            return this.RedirectToAction("Index", new { inventoryTypeId = inventoryViewModel.InventoryTypeId });
        }

        private InventoryEditOptions GetInventoryInventoryEditOptions(int customerId, int? inventoryTypeId)
        {
            List<ItemOverview> departments =
                this.OrganizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.PlaceService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.PlaceService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.PlaceService.GetRooms(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> computerTypes = this.PlaceService.GetComputerTypes(customerId, inventoryTypeId).OrderBy(x => x.Name).ToList();

            return new InventoryEditOptions(departments, buildings, floors, rooms, computerTypes);
        }

        private InventoryGridModel CreateInventoryGridModel(InventorySearchFilter filter, int inventoryTypeId, bool takeAllRecords = false)
        {
            var settings =
                _inventorySettingsService.GetInventoryFieldSettingsOverview(inventoryTypeId);

            /*-1: take all records */
            var inventoriesFilter = filter.CreateRequest(inventoryTypeId, takeAllRecords);
            var inventories = _inventoryService.GetInventories(inventoriesFilter);

            var viewModel = InventoryGridModel.BuildModel(inventories, settings, inventoryTypeId, filter.SortField);
            return viewModel;
        }
    }
}