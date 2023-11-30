using System;
using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Common.Tools;
using DH.Helpdesk.Services.Services.Orders;
using DH.Helpdesk.Web.Common.Tools.Files;
using DH.Helpdesk.Web.Enums;
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
	using Dal.Enums;
	using Infrastructure.Attributes;
	using System.IO;

	public class WorkstationController : InventoryBaseController
    {
        private readonly IInventoryService _inventoryService;
        private readonly IInventorySettingsService _inventorySettingsService;
        private readonly IComputerModulesService _computerModulesService;
        private readonly IComputerViewModelBuilder _computerViewModelBuilder;
        private readonly IComputerBuilder _computerBuilder;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly ITemporaryFilesCache _filesStore;
        private readonly IOrdersService _ordersService;
        private readonly ISettingService _settingsService;
		private readonly IGlobalSettingService _globalSettingService;
        private readonly IComputerCopyBuilder _computerCopyBuilder;

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
            IExcelFileComposer excelFileComposer,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            IOrdersService ordersService,
            ISettingService settingsService,
			IGlobalSettingService globalSettingService,
            IComputerCopyBuilder computerCopyBuilder)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            _filesStore = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Inventory);
            _inventoryService = inventoryService;
            _inventorySettingsService = inventorySettingsService;
            _computerModulesService = computerModulesService;
            _computerViewModelBuilder = computerViewModelBuilder;
            _computerBuilder = computerBuilder;
            _userPermissionsChecker = userPermissionsChecker;
            _ordersService = ordersService;
            _settingsService = settingsService;
			_globalSettingService = globalSettingService;
            _computerCopyBuilder = computerCopyBuilder;
        }

        [HttpGet]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public ViewResult Index()
        {
            var inventoryTypes = 
                _inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id, true, CreateInventoryTypeSeparatorItem());

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Workstations));

            var currentFilter =
                SessionFacade.FindPageFilters<WorkstationsSearchFilter>(WorkstationsSearchFilter.CreateFilterId()) ?? 
                WorkstationsSearchFilter.CreateDefault();

            var computerTypes = _computerModulesService.GetComputerTypes(SessionFacade.CurrentCustomer.Id);

            var domains = OrganizationService.GetDomains(SessionFacade.CurrentCustomer.Id);
            var regions = OrganizationService.GetRegions(SessionFacade.CurrentCustomer.Id);
            var departments = OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id, currentFilter.RegionId);
            var units = currentFilter.DepartmentId.HasValue 
                ? OrganizationService.GetOrganizationUnits(currentFilter.DepartmentId) 
                : OrganizationService.GetOrganizationUnitsByCustomer(SessionFacade.CurrentCustomer.Id);

            var settings =
                _inventorySettingsService.GetWorkstationFieldSettingsOverviewForFilter(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);

            var computerContractStatuses = _inventoryService.GetComputerContractStatuses(SessionFacade.CurrentCustomer.Id)
                .Translate()
                .OrderBy(x => x.Name)
                .ToList();

            var userHasInventoryAdminPermission = 
                _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);

            var viewModel = WorkstationSearchViewModel.BuildViewModel(
                currentFilter,
                domains,
                regions,
                departments,
                units,
                computerTypes,
                computerContractStatuses,
                settings,
                (int)CurrentModes.Workstations,
                inventoryTypes);

            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public PartialViewResult WorkstationsGrid(WorkstationsSearchFilter filter)
        {
            SessionFacade.SavePageFilters(WorkstationsSearchFilter.CreateFilterId(), filter);
            filter.RecordsCount = SearchFilter.RecordsOnPage;

            var viewModel = this.CreateInventoryGridModel(filter);
            return this.PartialView("InventoryGrid", viewModel);
        }

        [HttpGet]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public ActionResult Edit(int id, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);

           // if (!tabSettings.ComputersTabSetting.Show)
          //      return RedirectToTab(tabSettings.ComputersTabSetting, tabSettings, id, dialog, userId);

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);
            var readOnly = !userHasInventoryAdminPermission && dialog;
            var model = this._inventoryService.GetWorkstation(id);

            var options = this.GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id, model.OrganizationFields.DepartmentId, model.OrganizationFields.RegionId);

            var settings =
                _inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId, readOnly, true);

			var whiteList = _globalSettingService.GetFileUploadWhiteList();

			var computerEditModel = _computerViewModelBuilder.BuildViewModel(model, options, settings, whiteList);
            computerEditModel.IsForDialog = dialog;
            computerEditModel.UserId = userId;

            var viewModel = new ComputerEditViewModel(id, computerEditModel)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission,
                IsForDialog = dialog,
                UserId = userId,
                TabSettings = tabSettings,
                CurrentLanguageId = SessionFacade.CurrentLanguageId,
                CustomerId = SessionFacade.CurrentCustomer.Id,
				FileUploadWhiteList = whiteList
            };

            return View(viewModel);
        }

        [HttpPost]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public JsonResult UploadFile(string id, string name)
        {
            var fileName = Uri.UnescapeDataString(name);

            var uploadedFile = Request.Files[0];
            if (uploadedFile == null)
                throw new HttpException((int)HttpStatusCode.NotFound, null);

			var extension = Path.GetExtension(name);
			if (!_globalSettingService.IsExtensionInWhitelist(extension))
			{
				throw new ArgumentException($"File extension not valid: {name}");
			}


			var fileContent = uploadedFile.GetFileContent();

            if (GuidHelper.IsGuid(id))
            {
                _filesStore.ResetCacheForObject(id);
                _filesStore.AddFile(fileContent, name, id);
            }
            else
            {
                var computerId = Int32.Parse(id);
                _inventoryService.SaveWorkstationFile(computerId, fileName, fileContent);
            }

            return Json(new { success = true });
        }

        [HttpPost]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public ActionResult DeleteFile(string id, string name)
        {
            var fileName = Uri.UnescapeDataString(name);
            if (GuidHelper.IsGuid(id))
            {
                _filesStore.DeleteFile(fileName, id);
            }
            else
            {
                _inventoryService.DeleteWorkstationFile(Int32.Parse(id));
            }

            return Json(new {success = true});
        }

        //Workstantion/DownloadFile?id=<guid>&fileName=dfdf.jpg
        //Workstantion/DownloadFile?id=id
        [HttpGet]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public ActionResult DownloadFile(string id, string name)
        {
            var fileName = Uri.UnescapeDataString(name);
            
            byte[] fileContent;
            if (GuidHelper.IsGuid(id))
            {
                //todo: check file download in IE
                if (!_filesStore.FileExists(fileName, id))
                    throw new HttpException((int) HttpStatusCode.NotFound, null);

                fileContent = _filesStore.GetFileContent(fileName, id);
            }
            else
            {
                var computerId = Int32.Parse(id);
                var file = _inventoryService.GetWorkstationFile(computerId);
                fileName = file.FileName;
                fileContent = file?.Content;
            }

            if (fileContent == null)
            {
                return HttpNotFound();
            }

            return File(fileContent, MimeType.BinaryFile, Server.UrlEncode(fileName));
        }

        [HttpPost]
        [BadRequestOnNotValid]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public RedirectToRouteResult Edit(ComputerViewModel computerViewModel)
        {
            var businessModel = this._computerBuilder.BuildForUpdate(computerViewModel, OperationContext);
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

        [HttpPost]
        [UserPermissions(UserPermission.InventoryViewPermission)]
        public RedirectToRouteResult Copy(ComputerViewModel computerViewModel)
        {
            TempData["copyViewModel"] = computerViewModel;
            return RedirectToAction("New");
        }

        [HttpGet]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public ViewResult New(int? orderId = null)
        {
            var options = GetWorkstationEditOptions(SessionFacade.CurrentCustomer.Id, null, null);
            var settings =
                _inventorySettingsService.GetWorkstationFieldSettingsForModelEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId, 
                    false, 
                    true);

			var whiteList = _globalSettingService.GetFileUploadWhiteList();

            var viewModel = _computerViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id,
                whiteList);

            if (TempData.ContainsKey("copyViewModel"))
            {
                var sourceModel = TempData["copyViewModel"] as ComputerViewModel;
                if (sourceModel == null) throw new NullReferenceException("sourceModel can't be empty");
                viewModel = _computerCopyBuilder.CopyWorkstation(viewModel, sourceModel, this.OperationContext);
            }

            if (orderId.HasValue)
                ApplyOrderFields(orderId.Value, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public RedirectToRouteResult New(ComputerViewModel computerViewModel)
        {
            var computerFile = LoadTempFile(computerViewModel.DocumentFileKey); 
            var businessModel = _computerBuilder.BuildForAdd(computerViewModel, this.OperationContext, computerFile);
            _inventoryService.AddWorkstation(businessModel, this.OperationContext);
            
            return RedirectToAction("Index");
        }

        private ComputerFile LoadTempFile(string documentFileKey, bool deleteTempFile = true)
        {
            ComputerFile computerFile = null;
            if (!string.IsNullOrEmpty(documentFileKey))
            {
                var fileName = _filesStore.FindFileNames(documentFileKey).FirstOrDefault();
                if (!string.IsNullOrEmpty(fileName))
                {
                    var fileContent = _filesStore.GetFileContent(fileName, documentFileKey);
                    computerFile = new ComputerFile(fileName, fileContent);
                }

                if (deleteTempFile)
                {
                    _filesStore.ResetCacheForObject(documentFileKey);
                }
            }
            return computerFile;
        }

        [HttpPost]
		[UserPermissions(UserPermission.InventoryViewPermission)]
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
		[UserPermissions(UserPermission.InventoryViewPermission)]
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
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public ActionResult ComputerLogs(int computerId, bool dialog = false, string userId = null)
        {
            var tabSettings = _inventorySettingsService.GetWorkstationTabsSettingsForEdit(
                SessionFacade.CurrentCustomer.Id,
                SessionFacade.CurrentLanguageId);
            if (!tabSettings.ComputerLogsTabSetting.Show) return RedirectToTab(tabSettings.ComputerLogsTabSetting, tabSettings, computerId, dialog, userId);

            var userHasInventoryAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);

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
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public RedirectToRouteResult NewComputerLog(ComputerLogModel computerLogModel)
        {
            this._inventoryService.AddComputerLog(computerLogModel.CreateBusinessModel());
            return this.RedirectToAction("ComputerLogs", new { computerId = computerLogModel.ComputerId, dialog = computerLogModel.IsForDialog, userId = computerLogModel.UserId });
        }

        [HttpGet]
		[UserPermissions(UserPermission.InventoryViewPermission)]
		public RedirectToRouteResult DeleteComputerLog(int logId, int computerId, bool dialog = false, string userId = null)
        {
            this._inventoryService.DeleteComputerLog(logId);

            return this.RedirectToAction("ComputerLogs", new { computerId, dialog, userId });
        }

        [HttpGet]
        [UserPermissions(UserPermission.InventoryViewPermission)]
        public ContentResult GetPrice(int id)
        {
            return Content(this._inventoryService.GetComputerTypePrice(id).ToString());
        }

        [HttpGet]
		[UserPermissions(UserPermission.InventoryViewPermission)]
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

            var userHasInventoryAdminPermission = _userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);

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
		[UserPermissions(UserPermission.InventoryViewPermission)]
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
		[UserPermissions(UserPermission.InventoryViewPermission)]
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

        [HttpGet]
        [UserPermissions(UserPermission.InventoryViewPermission)]
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

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult ValidateMacAddress(int currentId, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return Json(true, JsonRequestBehavior.AllowGet);

            var result = !string.IsNullOrWhiteSpace(value) && _inventoryService.IsMacAddressUnique(currentId, value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult ValidateTheftmark(int currentId, string value)
        {
            var result = !string.IsNullOrWhiteSpace(value) && _inventoryService.IsTheftMarkUnique(currentId, value);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0)]
        public JsonResult ValidateComputerName(int currentId, string value)
        {
            bool isUnique = !string.IsNullOrWhiteSpace(value) && _inventoryService.IsComputerNameUnique(currentId, value);
            if (isUnique)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string errorMessage = string.Concat(" ", Translation.GetCoreTextTranslation("Datornamn"), " ", Translation.GetCoreTextTranslation("måste vara unikt"));
                return Json(new { valid = false, message = errorMessage, from = "nameCheck" }, JsonRequestBehavior.AllowGet);
            }
        }

        private InventoryGridModel CreateInventoryGridModel(WorkstationsSearchFilter filter)
        {
            var settings =
                this._inventorySettingsService.GetWorkstationFieldSettingsOverview(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            var cs = _settingsService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);
            var models = this._inventoryService.GetWorkstations(filter.CreateRequest(SessionFacade.CurrentCustomer.Id), cs.ComputerDepartmentSource.ToBool());
            models.ForEach(m =>
            {
                m.ContractFields.ContractStatusName = Translation.GetCoreTextTranslation(m.ContractFields.ContractStatusName);
                m.StateFields.StateName = Translation.GetCoreTextTranslation(m.StateFields.StateName);
            });

            var viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
        }

        private ComputerEditOptions GetWorkstationEditOptions(int customerId, int? departmentId, int? regionId)
        {
            var computerModels =
                _computerModulesService.GetComputerModels(customerId).OrderBy(x => x.Name).ToList();
            var computerTypes =
                _computerModulesService.GetComputerTypes(customerId).OrderBy(x => x.Name).ToList();
            var operatingSystems =
                _computerModulesService.GetOperatingSystems(customerId).OrderBy(x => x.Name).ToList();
            var processors = _computerModulesService.GetProcessors(customerId).OrderBy(x => x.Name).ToList();
            var rams = _computerModulesService.GetRams(customerId).OrderBy(x => x.Name).ToList();
            var netAdapters = _computerModulesService.GetNetAdapters(customerId).OrderBy(x => x.Name).ToList();
            var regions =
                OrganizationService.GetRegions(customerId).OrderBy(x => x.Name).ToList();
            var departments =
                OrganizationService.GetDepartments(customerId, regionId).OrderBy(x => x.Name).ToList();
            var domains =
                OrganizationService.GetDomains(customerId).OrderBy(x => x.Name).ToList();

            var ous = departmentId.HasValue ? OrganizationService.GetOrganizationUnits(departmentId.Value)
                : OrganizationService.GetOrganizationUnitsByCustomer(customerId).ToList();

            var buildings = PlaceService.GetBuildings(customerId).OrderBy(x => x.Name).ToList();
            var floors = PlaceService.GetFloors(customerId).OrderBy(x => x.Name).ToList();
            var rooms = PlaceService.GetRooms(customerId).OrderBy(x => x.Name).ToList();
            var computerContractStatuses = _inventoryService.GetComputerContractStatuses(SessionFacade.CurrentCustomer.Id)
                .Translate()
                .OrderBy(x => x.Name)
                .ToList();
            var computerStatuses = _inventoryService.GetWorkstationStatuses(SessionFacade.CurrentCustomer.Id)
                .Translate()
                .OrderBy(x => x.Name)
                .ToList();

            var computerResponse = new ComputerEditOptions(
                computerModels,
                computerTypes,
                operatingSystems,
                processors,
                rams,
                netAdapters,
                departments,
                regions,
                domains,
                ous,
                buildings,
                floors,
                rooms,
                computerContractStatuses,
                computerStatuses);

            return computerResponse;
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

        private void ApplyOrderFields(int orderId, ComputerViewModel viewModel)
        {
            var order = _ordersService.GetOrder(orderId);
            if (order != null)
            {
                viewModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension1.Value =
                    order.AccountingDimension1;
                viewModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension2.Value =
                    order.AccountingDimension2;
                viewModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension3.Value =
                    order.AccountingDimension3;
                viewModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension4.Value =
                    order.AccountingDimension4;
                viewModel.ContractFieldsViewModel.ContractFieldsModel.AccountingDimension5.Value =
                    order.AccountingDimension5;
                viewModel.ContactFieldsModel.Name.Value = order.ReceiverName;
                viewModel.ContactFieldsModel.Email.Value = order.ReceiverEMail;
                viewModel.ContactFieldsModel.Phone.Value = order.ReceiverPhone;
                viewModel.OrganizationFieldsViewModel.OrganizationFieldsModel.DepartmentId.Value = order.Department_Id;
                viewModel.OrganizationFieldsViewModel.OrganizationFieldsModel.UnitId.Value = order.OU_Id;
                viewModel.PlaceFieldsViewModel.PlaceFieldsModel.Address.Value = order.DeliveryAddress;
                viewModel.PlaceFieldsViewModel.PlaceFieldsModel.PostalCode.Value = order.DeliveryPostalCode;
                viewModel.PlaceFieldsViewModel.PlaceFieldsModel.PostalAddress.Value = order.DeliveryPostalAddress;
                viewModel.PlaceFieldsViewModel.PlaceFieldsModel.Location.Value = order.DeliveryLocation;
                viewModel.PlaceFieldsViewModel.PlaceFieldsModel.Location2.Value = order.OrderRow6;
            }
        }
    }
}