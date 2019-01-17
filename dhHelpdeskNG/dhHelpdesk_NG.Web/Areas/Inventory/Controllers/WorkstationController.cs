using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Web.Models.Shared;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Response.Inventory;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.OptionsAggregates;
    using DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    public class WorkstationController : InventoryBaseController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IInventorySettingsService _inventorySettingsService;
        private readonly IComputerModulesService _computerModulesService;
        private readonly IComputerViewModelBuilder _computerViewModelBuilder;
        private readonly IComputerBuilder _computerBuilder;
        private readonly IUserPermissionsChecker _userPermissionsChecker;

        public WorkstationController(
            IMasterDataService masterDataService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService,
            IComputerModulesService computerModulesService,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IComputerViewModelBuilder computerViewModelBuilder,
            IComputerBuilder computerBuilder,
            IExportFileNameFormatter exportFileNameFormatter,
            IUserPermissionsChecker userPermissionsChecker,
            IExcelFileComposer excelFileComposer)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this._inventoryService = inventoryService;
            this._inventorySettingsService = inventorySettingsService;
            this._computerModulesService = computerModulesService;
            this._computerViewModelBuilder = computerViewModelBuilder;
            this._computerBuilder = computerBuilder;
            this._userPermissionsChecker = userPermissionsChecker;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var inventoryTypes = 
                _inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id, true, CreateInventoryTypeSeparatorItem());

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Workstations));

            var currentFilter =
                SessionFacade.FindPageFilters<WorkstationsSearchFilter>(WorkstationsSearchFilter.CreateFilterId()) ?? 
                WorkstationsSearchFilter.CreateDefault();

            var computerTypes = _computerModulesService.GetComputerTypes(SessionFacade.CurrentCustomer.Id);

            var regions = OrganizationService.GetRegions(SessionFacade.CurrentCustomer.Id);
            var departments = OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id, currentFilter.RegionId);

            var settings =
                _inventorySettingsService.GetWorkstationFieldSettingsOverviewForFilter(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            var userHasInventoryAdminPermission = 
                _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            var viewModel = WorkstationSearchViewModel.BuildViewModel(
                currentFilter,
                regions,
                departments,
                computerTypes,
                settings,
                (int)CurrentModes.Workstations,
                inventoryTypes);

            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            SessionFacade.SavePageFilters(WorkstationsSearchFilter.CreateFilterId(), filter);
            filter.RecordsCount = SearchFilter.RecordsOnPage;

            InventoryGridModel viewModel = this.CreateInventoryGridModel(filter);
            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            if (!tabSettings.ComputersTabSetting.Show) return RedirectToTab(tabSettings.ComputersTabSetting, tabSettings, id, dialog, userId);

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);
            var readOnly = !userHasInventoryAdminPermission && dialog;
            var model = this._inventoryService.GetWorkstation(id);

            var options = this.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);

            var settings =
                _inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId, readOnly);

            var computerEditModel = this._computerViewModelBuilder.BuildViewModel(model, options, settings);
            computerEditModel.IsForDialog = dialog;
            computerEditModel.UserId = userId;

            var viewModel = new ComputerEditViewModel(id, computerEditModel)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission,
                IsForDialog = dialog,
                UserId = userId,
                TabSettings = tabSettings
            };

            return View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Edit(ComputerViewModel computerViewModel)
        {
            ComputerForUpdate businessModel = this._computerBuilder.BuildForUpdate(computerViewModel, OperationContext);
            this._inventoryService.UpdateWorkstation(businessModel, this.OperationContext);
            if (computerViewModel.IsForDialog)
            {
                if (!string.IsNullOrEmpty(computerViewModel.UserId))
                {
                    return RedirectToAction("RelatedInventoryFull", new {userId = computerViewModel.UserId});
                }
                return RedirectToAction("Edit", new { id = computerViewModel.Id, dialog = computerViewModel.IsForDialog });
            }
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult New()
        {
            ComputerEditOptions options = this.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id);
            ComputerFieldsSettingsForModelEdit settings =
                this._inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            ComputerViewModel viewModel = this._computerViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(ComputerViewModel computerViewModel)
        {
            ComputerForInsert businessModel = this._computerBuilder.BuildForAdd(computerViewModel, this.OperationContext);
            this._inventoryService.AddWorkstation(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this._inventoryService.DeleteWorkstation(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Storage(int computerId, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            if (!tabSettings.StorageTabSetting.Show) return RedirectToTab(tabSettings.StorageTabSetting, tabSettings, computerId, dialog, userId);

            var models = _computerModulesService.GetComputerLogicalDrive(computerId);

            var viewModel = new StorageViewModel(computerId, models)
            {
                IsForDialog = dialog,
                UserId = userId,
                TabSettings = tabSettings
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Software(int computerId, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            if (!tabSettings.SoftwareTabSetting.Show) return RedirectToTab(tabSettings.SoftwareTabSetting, tabSettings, computerId, dialog, userId);

            var models = _computerModulesService.GetComputerSoftware(computerId);

            var viewModel = new SoftwareViewModel(computerId, models)
            {
                IsForDialog = dialog,
                UserId = userId,
                TabSettings = tabSettings
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult HotFixes(int computerId, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            if (!tabSettings.HotFixesTabSetting.Show) return RedirectToTab(tabSettings.HotFixesTabSetting, tabSettings, computerId, dialog, userId);

            var models = _computerModulesService.GetComputerSoftware(computerId);

            var viewModel = new HotfixViewModel(computerId, models)
            {
                IsForDialog = dialog,
                UserId = userId,
                TabSettings = tabSettings
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult ComputerLogs(int computerId, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            if (!tabSettings.ComputerLogsTabSetting.Show) return RedirectToTab(tabSettings.ComputerLogsTabSetting, tabSettings, computerId, dialog, userId);

            var userHasInventoryAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            var models = this._inventoryService.GetWorkstationLogOverviews(computerId);

            var viewModel = new LogsViewModel(computerId, models, dialog, userId)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission,
                IsForDialog = dialog,
                UserId = userId,
                TabSettings = tabSettings
            };

            return View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewComputerLog(ComputerLogModel computerLogModel)
        {
            this._inventoryService.AddComputerLog(computerLogModel.CreateBusinessModel());
            return this.RedirectToAction("ComputerLogs", new { computerId = computerLogModel.ComputerId, dialog = computerLogModel.IsForDialog, userId = computerLogModel.UserId });
        }

        [HttpGet]
        public RedirectToRouteResult DeleteComputerLog(int logId, int computerId, bool dialog = false, string userId = null)
        {
            this._inventoryService.DeleteComputerLog(logId);

            return this.RedirectToAction("ComputerLogs", new { computerId, dialog, userId });
        }

        [HttpGet]
        public ActionResult Accessories(int computerId, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            if (!tabSettings.AccessoriesTabSetting.Show) return RedirectToTab(tabSettings.AccessoriesTabSetting, tabSettings, computerId, dialog, userId);

            var inventory =
                _inventoryService.GetConnectedToComputerInventories(computerId);
            var invetoryTypeIds = inventory.Overviews.Select(x => x.InventoryTypeId).ToList();
            var settings =
                _inventorySettingsService.GetInventoryFieldSettingsOverview(invetoryTypeIds);
            var inventoryGridModels = InventoryGridModel.BuildModels(inventory, settings);

            //todo: Check if standard items and separator item is required 
            var inventoryTypes = _inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id, false);

            // todo
            var selected = inventoryTypes.Min(x => x.Value.ToNullableInt32());
            var inventories = selected.HasValue
                                                 ? _inventoryService.GetNotConnectedInventory(
                                                     selected.Value,
                                                     computerId)
                                                 : new List<ItemOverview>();

            var userHasInventoryAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            var viewModel = AccesoriesViewModel.BuildViewModel(
                computerId,
                selected,
                inventoryTypes,
                inventories,
                inventoryGridModels);

            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;
            viewModel.IsForDialog = dialog;
            viewModel.UserId = userId;
            viewModel.TabSettings = tabSettings;
            return View(viewModel);
        }

        [HttpGet]
        public JsonResult SearchNotConnectedInventory(int? selected, int computerId)
        {
            if (!selected.HasValue)
            {
                return this.Json(new { });
            }

            var models = this._inventoryService.GetNotConnectedInventory(selected.Value, computerId);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public RedirectToRouteResult ConnectInventoryToComputer(int? inventoryId, int computerId, bool dialog = false, string userId = null)
        {
            if (!inventoryId.HasValue)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            this._inventoryService.ConnectInventoryToComputer(inventoryId.Value, computerId);

            return this.RedirectToAction("Accessories", new { computerId, dialog, userId });
        }

        [HttpGet]
        public RedirectToRouteResult DeleteInventoryFromComputer(int computerId, int inventoryId, bool dialog = false, string userId = null)
        {
            this._inventoryService.RemoveInventoryFromComputer(inventoryId, computerId);

            return this.RedirectToAction("Accessories", new { computerId, dialog, userId });
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile()
        {
            var workstationFilter =
                SessionFacade.FindPageFilters<WorkstationsSearchFilter>(WorkstationsSearchFilter.CreateFilterId()) ?? WorkstationsSearchFilter.CreateDefault();
            
            //set null to get all records for export but do not save filter to keep limit for UI
            workstationFilter.RecordsCount = null;

            var gridModel = CreateInventoryGridModel(workstationFilter);
            var workSheetName = CurrentModes.Workstations.ToString();

            var file = this.CreateExcelReport(workSheetName, gridModel.Headers, gridModel.Inventories);

            return file;
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SearchComputerUsers(string query, string searchKey)
        {
            List<ComputerUserOverview> models = this._inventoryService.GetComputerUsers(
                OperationContext.CustomerId,
                query);

            return Json(new { searchKey = searchKey, result = models });
        }

        [HttpGet]
        public PartialViewResult SearchComputerUserHistory(int computerId)
        {
            List<ComputerUserOverview> models = this._inventoryService.GetComputerUserHistory(computerId);

            return this.PartialView("UserHistoryDialog", models);
        }

        private InventoryGridModel CreateInventoryGridModel(WorkstationsSearchFilter filter)
        {
            var settings =
                this._inventorySettingsService.GetWorkstationFieldSettingsOverview(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            var models = this._inventoryService.GetWorkstations(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            InventoryGridModel viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }

        private ComputerEditOptions GetWorkstationEditOptions(int customerId)
        {
            List<ItemOverview> computerModels =
                this._computerModulesService.GetComputerModels().OrderBy(x => x.Name).ToList();
            List<ItemOverview> computerTypes =
                this._computerModulesService.GetComputerTypes(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> operatingSystems =
                this._computerModulesService.GetOperatingSystems().OrderBy(x => x.Name).ToList();
            List<ItemOverview> processors = this._computerModulesService.GetProcessors().OrderBy(x => x.Name).ToList();
            List<ItemOverview> rams = this._computerModulesService.GetRams().OrderBy(x => x.Name).ToList();
            List<ItemOverview> netAdapters = this._computerModulesService.GetNetAdapters().OrderBy(x => x.Name).ToList();
            List<ItemOverview> departments =
                this.OrganizationService.GetDepartments(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> domains =
                this.OrganizationService.GetDomains(customerId).OrderBy(x => x.Name).ToList();

            List<ItemOverview> ous = this.OrganizationService.GetCustomerOUs(customerId)                                                             
                                                             .Select(o=> new ItemOverview(o.Name, o.Id.ToString()))
                                                             .OrderBy(x => x.Name).ToList();

            List<ItemOverview> buildings = this.PlaceService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> floors = this.PlaceService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            List<ItemOverview> rooms = this.PlaceService.GetRooms(customerId).OrderBy(x => x.Name).ToList();

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

        [HttpGet]
        public ActionResult RelatedInventoryFull(string userId)
        {
            var sortField = new SortFieldModel
            {
                Name = BusinessData.Enums.Inventory.Fields.Computer.WorkstationFields.Name,
                SortBy = SortBy.Ascending
            };
            var settings = _inventorySettingsService.GetWorkstationFieldSettingsOverview(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);
            var models = _inventoryService.GetRelatedInventory(SessionFacade.CurrentCustomer.Id, userId);
            var viewModel = InventoryGridModel.BuildModel(models, settings, sortField);
            ViewData.Add(new KeyValuePair<string, object>("UserId", userId));
            return View(viewModel);
        }

        private RedirectToRouteResult RedirectToTab(TabSetting current, WorkstationTabsSettings tabSettings, int id, bool dialog, string userId)
        {
            var routes = new Dictionary<TabSetting, RedirectToRouteResult>()
            {
                { tabSettings.ComputersTabSetting, RedirectToAction("Edit", new { id = id, dialog = dialog, userId = userId }) },
                { tabSettings.StorageTabSetting, RedirectToAction("Storage", new { computerId = id, dialog = dialog, userId = userId }) },
                { tabSettings.SoftwareTabSetting, RedirectToAction("Software", new { computerId = id, dialog = dialog, userId = userId }) },
                { tabSettings.HotFixesTabSetting, RedirectToAction("HotFixes", new { computerId = id, dialog = dialog, userId = userId }) },
                { tabSettings.ComputerLogsTabSetting, RedirectToAction("ComputerLogs", new { computerId = id, dialog = dialog, userId = userId }) },
                { tabSettings.AccessoriesTabSetting, RedirectToAction("Accessories", new { computerId = id, dialog = dialog, userId = userId }) }
            };

            var found = false;
            foreach (var route in routes)
            {
                if (route.Key.TabField == current.TabField)
                {
                    found = true;
                    continue;
                }

                if (found && route.Key.Show)
                    return route.Value;
            }

            return RedirectToAction("Index", "Workstation", new { Area = "Inventory"});
        }
    }
}