using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;

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
    using Infrastructure.Attributes;
    using System;
    using System.Web;
    using System.Net;
    using System.IO;
    using Infrastructure.Extensions;
    using Helpdesk.Common.Tools;
    using Enums;
    using Common.Tools.Files;

    public class ServerController : InventoryBaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IInventorySettingsService inventorySettingsService;

        private readonly IComputerModulesService computerModulesService;

        private readonly IServerViewModelBuilder serverViewModelBuilder;

        private readonly IServerBuilder serverBuilder;

        private readonly IUserPermissionsChecker _userPermissionsChecker;

        private readonly IGlobalSettingService _globalSettingService;

        private readonly ITemporaryFilesCache _filesStore;


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
            IExportFileNameFormatter exportFileNameFormatter,
            ITemporaryFilesCacheFactory userTemporaryFilesStorageFactory,
            IGlobalSettingService globalSettingService)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            _filesStore = userTemporaryFilesStorageFactory.CreateForModule(ModuleName.Inventory);
            this.inventoryService = inventoryService;
            this.inventorySettingsService = inventorySettingsService;
            this.computerModulesService = computerModulesService;
            this.serverViewModelBuilder = serverViewModelBuilder;
            this.serverBuilder = serverBuilder;
            this._userPermissionsChecker = userPermissionsChecker;
            this._globalSettingService = globalSettingService;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var inventoryTypes =
                this.inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id, true, CreateInventoryTypeSeparatorItem());

            SessionFacade.SavePageFilters(TabName.Inventories, new InventoriesModeFilter((int)CurrentModes.Servers));
            var currentFilter =
                SessionFacade.FindPageFilters<ServerSearchFilter>(ServerSearchFilter.CreateFilterId()) ?? 
                ServerSearchFilter.CreateDefault();

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);

            var viewModel = new ServerSearchViewModel((int) CurrentModes.Servers, inventoryTypes, currentFilter)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission
            };

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ServersGrid(ServerSearchFilter filter)
        {
            SessionFacade.SavePageFilters(ServerSearchFilter.CreateFilterId(), filter);
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

            var whiteList = _globalSettingService.GetFileUploadWhiteList();

            ServerViewModel viewModel = this.serverViewModelBuilder.BuildViewModel(
                options,
                settings,
                SessionFacade.CurrentCustomer.Id,
                whiteList);

            return this.View(viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult New(ServerViewModel serverViewModel)
        {
            var computerFile = LoadTempFile(serverViewModel.DocumentFileKey); 
            ServerForInsert businessModel = this.serverBuilder.BuildForAdd(serverViewModel, this.OperationContext, computerFile);
            this.inventoryService.AddServer(businessModel, this.OperationContext);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int id, bool dialog = false)
        {
            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryAdminPermission);
            var readOnly = !userHasInventoryAdminPermission && dialog;

            var whiteList = _globalSettingService.GetFileUploadWhiteList();

            ServerForRead model = this.inventoryService.GetServer(id);
            ServerEditOptions options = this.GetServerEditOptions(SessionFacade.CurrentCustomer.Id);
            ServerFieldsSettingsForModelEdit settings = inventorySettingsService.GetServerFieldSettingsForModelEdit(
                SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId, readOnly);

            ServerViewModel serverEditModel = this.serverViewModelBuilder.BuildViewModel(model, options, settings, whiteList);
            serverEditModel.IsForDialog = dialog;

            

            var viewModel = new ServerEditViewModel(id, serverEditModel)
            {
                UserHasInventoryAdminPermission = userHasInventoryAdminPermission,
                IsForDialog = dialog,
                FileUploadWhiteList = whiteList
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
                inventoryService.SaveServerFile(computerId, fileName, fileContent);
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
                inventoryService.DeleteServerFile(Int32.Parse(id));
            }

            return Json(new { success = true });
        }

        //Server/DownloadFile?id=<guid>&fileName=dfdf.jpg
        //Server/DownloadFile?id=id
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
                    throw new HttpException((int)HttpStatusCode.NotFound, null);

                fileContent = _filesStore.GetFileContent(fileName, id);
            }
            else
            {
                var computerId = Int32.Parse(id);
                var file = inventoryService.GetServerFile(computerId);
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
        public RedirectToRouteResult Delete(int id)
        {
            this.inventoryService.DeleteServer(id);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public FileContentResult ExportGridToExcelFile()
        {
            var serverFilter = 
                SessionFacade.FindPageFilters<ServerSearchFilter>(ServerSearchFilter.CreateFilterId()) ?? 
                ServerSearchFilter.CreateDefault();
            
            // do not save - UI need to have a limitation
            serverFilter.RecordsCount = null;

            var gridModel = this.CreateInventoryGridModel(serverFilter);

            var file = CreateExcelReport(CurrentModes.Servers.ToString(), gridModel.Headers, gridModel.Inventories);
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
            var settings = 
                inventorySettingsService.GetServerFieldSettingsOverview(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            var models = this.inventoryService.GetServers(filter.CreateRequest(SessionFacade.CurrentCustomer.Id));

            var viewModel = InventoryGridModel.BuildModel(models, settings, filter.SortField);
            return viewModel;
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
    }
}