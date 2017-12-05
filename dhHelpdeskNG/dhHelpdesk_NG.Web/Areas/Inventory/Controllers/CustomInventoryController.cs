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
        private readonly IInventorySettingsService inventorySettingsService;

        private readonly IInventoryService inventoryService;

        private readonly IInventoryViewModelBuilder inventoryViewModelBuilder;

        private readonly IDynamicsFieldsModelBuilder dynamicsFieldsModelBuilder;

        private readonly IInventoryModelBuilder inventoryModelBuilder;

        private readonly IInventoryValueBuilder inventoryValueBuilder;

        private readonly IUserPermissionsChecker _userPermissionsChecker;

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
            IUserPermissionsChecker userPermissionsChecker)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this.inventorySettingsService = inventorySettingsService;
            this.inventoryService = inventoryService;
            this.inventoryViewModelBuilder = inventoryViewModelBuilder;
            this.dynamicsFieldsModelBuilder = dynamicsFieldsModelBuilder;
            this.inventoryModelBuilder = inventoryModelBuilder;
            this.inventoryValueBuilder = inventoryValueBuilder;
            this._userPermissionsChecker = userPermissionsChecker;
        }

        [HttpGet]
        public ViewResult Index(int inventoryTypeId)
        {
            List<ItemOverview> inventoryTypes = this.inventoryService.GetInventoryTypes(
                SessionFacade.CurrentCustomer.Id);

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter(inventoryTypeId));
            InventorySearchFilter currentFilter =
                SessionFacade.FindPageFilters<InventorySearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.CustomType.ToString()))
                ?? InventorySearchFilter.CreateDefault(inventoryTypeId);

            List<ItemOverview> departments = this.OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);
            InventoryFieldsSettingsOverviewForFilter settings =
                this.inventorySettingsService.GetInventoryFieldSettingsOverviewForFilter(inventoryTypeId);

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            InventorySearchViewModel viewModel = InventorySearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                settings,
                inventoryTypeId,
                inventoryTypeId,
                inventoryTypes);

            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoriesGrid(InventorySearchFilter filter, int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(
                this.CreateFilterId(TabName.Inventories, InventoryFilterMode.CustomType.ToString()),
                filter);

            InventoryGridModel viewModel = this.CreateInventoryGridModel(filter, inventoryTypeId);

            return this.PartialView("CustomInventoryGrid", viewModel);
        }

        [HttpGet]
        public ViewResult New(int inventoryTypeId)
        {
            InventoryType inventoryType = this.inventoryService.GetInventoryType(inventoryTypeId);
            InventoryFieldSettingsForModelEditResponse settings =
                this.inventorySettingsService.GetInventoryFieldSettingsForModelEdit(inventoryTypeId);
            InventoryEditOptions options = this.GetInventoryInventoryEditOptions(SessionFacade.CurrentCustomer.Id);
            List<TypeGroupModel> typeGroupModels = this.inventoryService.GetTypeGroupModels(inventoryTypeId);

            InventoryViewModel inventoryViewModel = this.inventoryViewModelBuilder.BuildViewModel(
                options,
                settings.InventoryFieldSettingsForModelEdit,
                inventoryTypeId,
                SessionFacade.CurrentCustomer.Id);

            List<DynamicFieldModel> dynamicFieldsModel =
                this.dynamicsFieldsModelBuilder.BuildViewModel(settings.InventoryDynamicFieldSettingForModelEditData);
            inventoryViewModel.Name = inventoryType.Name;

            var viewModel = new InventoryEditViewModel(inventoryViewModel, dynamicFieldsModel, typeGroupModels);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(
            InventoryViewModel inventoryViewModel,
            List<DynamicFieldModel> allModels)
        {
            InventoryForInsert businessModel = this.inventoryModelBuilder.BuildForAdd(
                inventoryViewModel,
                OperationContext);
            this.inventoryService.AddInventory(businessModel);
            List<InventoryValueForWrite> dynamicBusinessModels =
                this.inventoryValueBuilder.BuildForWrite(businessModel.Id, allModels);
            this.inventoryService.AddDynamicFieldsValuesInventory(dynamicBusinessModels);

            return this.RedirectToAction("Index", new { inventoryTypeId = inventoryViewModel.InventoryTypeId });
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int inventoryTypeId, int id)
        {
            this.inventoryService.DeleteInventory(id);

            return this.RedirectToAction("Index", new { inventoryTypeId });
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile(int inventoryTypeId)
        {
            InventorySearchFilter inventoryFilter =
                SessionFacade.FindPageFilters<InventorySearchFilter>(
                    this.CreateFilterId(TabName.Inventories, InventoryFilterMode.CustomType.ToString()))
                ?? InventorySearchFilter.CreateDefault(inventoryTypeId);
            InventoryGridModel gridModel = this.CreateInventoryGridModel(inventoryFilter, inventoryTypeId, true);

            // todo move into invetory overview query
            InventoryType inventoryType = this.inventoryService.GetInventoryType(inventoryTypeId);
            string workSheetName = inventoryType.Name;
            FileContentResult file = this.CreateExcelReport(workSheetName, gridModel.Headers, gridModel.Inventories);

            return file;
        }

        [HttpGet]
        public ViewResult Edit(int id, int inventoryTypeId)
        {
            InventoryOverviewResponse model = this.inventoryService.GetInventory(id);
            InventoryFieldSettingsForModelEditResponse settings = this.inventorySettingsService.GetInventoryFieldSettingsForModelEdit(inventoryTypeId);
            InventoryEditOptions options = this.GetInventoryInventoryEditOptions(SessionFacade.CurrentCustomer.Id);
            List<TypeGroupModel> typeGroupModels = this.inventoryService.GetTypeGroupModels(inventoryTypeId);

            InventoryViewModel inventoryViewModel = this.inventoryViewModelBuilder.BuildViewModel(
                model.Inventory,
                options,
                settings.InventoryFieldSettingsForModelEdit);
            List<DynamicFieldModel> dynamicFieldsModel = this.dynamicsFieldsModelBuilder.BuildViewModel(
                model.DynamicData,
                settings.InventoryDynamicFieldSettingForModelEditData,
                id);
            inventoryViewModel.Name = model.Inventory.InventoryTypeName;

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            var viewModel = new InventoryEditViewModel(inventoryViewModel, dynamicFieldsModel, typeGroupModels);
            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(
            InventoryViewModel inventoryViewModel,
            List<DynamicFieldModel> allModels)
        {
            InventoryForUpdate businessModel = this.inventoryModelBuilder.BuildForUpdate(inventoryViewModel, OperationContext);
            List<InventoryValueForWrite> dynamicBusinessModels = this.inventoryValueBuilder.BuildForWrite(
                inventoryViewModel.Id,
                allModels);
            this.inventoryService.UpdateInventory(
                businessModel,
                dynamicBusinessModels,
                inventoryViewModel.InventoryTypeId);

            return this.RedirectToAction("Index", new { inventoryTypeId = inventoryViewModel.InventoryTypeId });
        }

        private InventoryEditOptions GetInventoryInventoryEditOptions(int customerId)
        {
            List<ItemOverview> departments =
                this.OrganizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> buildings = this.PlaceService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.PlaceService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.PlaceService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

            return new InventoryEditOptions(departments, buildings, floors, rooms);
        }

        private InventoryGridModel CreateInventoryGridModel(InventorySearchFilter filter, int inventoryTypeId, bool takeAllRecords = false)
        {
            InventoryFieldSettingsOverviewResponse settings =
                this.inventorySettingsService.GetInventoryFieldSettingsOverview(inventoryTypeId);

            /*-1: take all records */
            var _filter = filter.CreateRequest(inventoryTypeId, takeAllRecords? (int?) -1 :null);
            InventoriesOverviewResponse models =
                inventoryService.GetInventories(_filter);

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(
                models,
                settings,
                inventoryTypeId,
                filter.SortField);
            return viewModel;
        }
    }
}