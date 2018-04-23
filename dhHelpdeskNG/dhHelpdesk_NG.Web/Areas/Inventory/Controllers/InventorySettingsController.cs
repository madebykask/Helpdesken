using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Inventory;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server;
    using DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    public class InventorySettingsController : BaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IInventorySettingsService inventorySettingsService;

        private readonly IComputerFieldsSettingsViewModelBuilder computerFieldsSettingsViewModelBuilder;

        private readonly IServerFieldsSettingsViewModelBuilder serverFieldsSettingsViewModelBuilder;

        private readonly IPrinterFieldsSettingsViewModelBuilder printerFieldsSettingsViewModelBuilder;

        private readonly IInventoryFieldSettingsEditViewModelBuilder inventoryFieldSettingsEditViewModelBuilder;

        private readonly IComputerFieldsSettingsBuilder computerFieldsSettingsBuilder;

        private readonly IServerFieldsSettingsBuilder serverFieldsSettingsBuilder;

        private readonly IPrinterFieldsSettingsBuilder printerFieldsSettingsBuilder;

        private readonly IInventoryFieldsSettingsBuilder inventoryFieldsSettingsBuilder;

        private readonly ILanguageService languageService;

        private readonly IUserPermissionsChecker _userPermissionsChecker;

        public InventorySettingsController(
            IMasterDataService masterDataService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService,
            IComputerFieldsSettingsViewModelBuilder computerFieldsSettingsViewModelBuilder,
            IServerFieldsSettingsViewModelBuilder serverFieldsSettingsViewModelBuilder,
            IPrinterFieldsSettingsViewModelBuilder printerFieldsSettingsViewModelBuilder,
            IInventoryFieldSettingsEditViewModelBuilder inventoryFieldSettingsEditViewModelBuilder,
            IComputerFieldsSettingsBuilder computerFieldsSettingsBuilder,
            IServerFieldsSettingsBuilder serverFieldsSettingsBuilder,
            IPrinterFieldsSettingsBuilder printerFieldsSettingsBuilder,
            IInventoryFieldsSettingsBuilder inventoryFieldsSettingsBuilder,
            IUserPermissionsChecker userPermissionsChecker,
            ILanguageService languageService)
            : base(masterDataService)
        {
            this.inventoryService = inventoryService;
            this.inventorySettingsService = inventorySettingsService;
            this.computerFieldsSettingsViewModelBuilder = computerFieldsSettingsViewModelBuilder;
            this.serverFieldsSettingsViewModelBuilder = serverFieldsSettingsViewModelBuilder;
            this.printerFieldsSettingsViewModelBuilder = printerFieldsSettingsViewModelBuilder;
            this.inventoryFieldSettingsEditViewModelBuilder = inventoryFieldSettingsEditViewModelBuilder;
            this.computerFieldsSettingsBuilder = computerFieldsSettingsBuilder;
            this.serverFieldsSettingsBuilder = serverFieldsSettingsBuilder;
            this.printerFieldsSettingsBuilder = printerFieldsSettingsBuilder;
            this.inventoryFieldsSettingsBuilder = inventoryFieldsSettingsBuilder;
            this.languageService = languageService;
            this._userPermissionsChecker = userPermissionsChecker;
        }

        [HttpGet]
        public ViewResult Index()
        {
            List<ItemOverview> inventoryTypes = this.inventoryService.GetInventoryTypes(
                SessionFacade.CurrentCustomer.Id);

            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            var viewModel = new SettingsIndexViewModel(inventoryTypes);
            viewModel.UserHasInventoryAdminPermission = userHasInventoryAdminPermission;

            return this.View(viewModel);
        }

        [HttpGet]
        public ViewResult EditSettings(int inventoryTypeId)
        {
            var userHasInventoryAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.InventoryPermission);

            switch ((CurrentModes)inventoryTypeId)
            {
                case CurrentModes.Workstations:
                case CurrentModes.Servers:
                case CurrentModes.Printers:
                    return this.View("EditSettings", inventoryTypeId);

                default:
                    return this.View("EditInventorySettings", inventoryTypeId);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult RenderContent(int inventoryTypeId)
        {
            switch ((CurrentModes)inventoryTypeId)
            {
                case CurrentModes.Workstations:
                    return this.WorkstationSettings(SessionFacade.CurrentLanguageId);

                case CurrentModes.Servers:
                    return this.ServerSettings(SessionFacade.CurrentLanguageId);

                case CurrentModes.Printers:
                    return this.PrinterSettings(SessionFacade.CurrentLanguageId);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet]
        public PartialViewResult WorkstationSettings(int languageId)
        {
            var settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsForEdit(
                    SessionFacade.CurrentCustomer.Id,
                    languageId);
            var langauges = this.languageService.GetActiveOverviews();
            var viewModel = this.computerFieldsSettingsViewModelBuilder.BuildViewModel(
                settings,
                langauges,
                languageId);

            return this.PartialView("WorkstationSettings", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void WorkstationSettings(ComputerFieldsSettingsViewModel model)
        {
            var businessModel = this.computerFieldsSettingsBuilder.BuildViewModel(
                model,
                SessionFacade.CurrentCustomer.Id);

            this.inventorySettingsService.UpdateWorkstationFieldsSettings(businessModel);
        }

        [HttpGet]
        public PartialViewResult ServerSettings(int languageId)
        {
            var settings =
                this.inventorySettingsService.GetServerFieldSettingsForEdit(
                    SessionFacade.CurrentCustomer.Id,
                    languageId);
            var langauges = this.languageService.GetActiveOverviews();
            var viewModel = this.serverFieldsSettingsViewModelBuilder.BuildViewModel(
                settings,
                langauges,
                languageId);

            return this.PartialView("ServerSettings", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void ServerSettings(ServerFieldsSettingsViewModel model)
        {
            var businessModel = this.serverFieldsSettingsBuilder.BuildViewModel(
                model,
                SessionFacade.CurrentCustomer.Id);

            this.inventorySettingsService.UpdateServerFieldsSettings(businessModel);
        }

        [HttpGet]
        public PartialViewResult PrinterSettings(int languageId)
        {
            var settings =
                this.inventorySettingsService.GetPrinterFieldSettingsForEdit(SessionFacade.CurrentCustomer.Id, languageId);

            var langauges = this.languageService.GetActiveOverviews();

            var viewModel = 
                this.printerFieldsSettingsViewModelBuilder.BuildViewModel(settings, langauges, languageId);

            return this.PartialView("PrinterSettings", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void PrinterSettings(PrinterFieldsSettingsViewModel model)
        {
            var businessModel = 
                this.printerFieldsSettingsBuilder.BuildViewModel(model, SessionFacade.CurrentCustomer.Id);

            this.inventorySettingsService.UpdatePrinterFieldsSettings(businessModel);
        }

        [HttpGet]
        public PartialViewResult InventorySettings(int inventoryTypeId)
        {
            var model = this.inventorySettingsService.GetInventoryFieldSettingsForEdit(inventoryTypeId);
            var typeGroups = this.inventoryService.GetTypeGroupModels(inventoryTypeId);
            var inventoryTypeModel = this.inventoryService.GetInventoryType(inventoryTypeId);

            var viewModel = this.inventoryFieldSettingsEditViewModelBuilder.BuildViewModel(inventoryTypeModel, model, typeGroups);
            return this.PartialView("InventorySettings", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult InventorySettings(InventoryFieldSettingsEditViewModel model)
        {
            this.UpdateInventoryType(model.InventoryTypeModel);

            this.UpdateInventoryFieldsSettings(
                model.InventoryTypeModel.Id,
                model.InventoryFieldSettingsViewModel.DefaultSettings);

            var newSetting =
                model.InventoryFieldSettingsViewModel.NewDynamicFieldViewModel.InventoryDynamicFieldSettingModel;

            this.AddDynamicFieldSetting(model.InventoryTypeModel.Id, newSetting);

            this.UpdateDynamicFieldsSettings(
                model.InventoryFieldSettingsViewModel.InventoryDynamicFieldViewModelSettings);

            return this.RedirectToAction("InventorySettings", new { inventoryTypeId = model.InventoryTypeModel.Id });
        }

        [HttpGet]
        public ViewResult NewCustomInventoryType()
        {
            return this.View("NewInventorySettings");
        }

        [HttpGet]
        public PartialViewResult NewInventorySettings()
        {
            // todo
            var typeGroups = new List<TypeGroupModel>();

            var viewModel = this.inventoryFieldSettingsEditViewModelBuilder.BuildDefaultViewModel(typeGroups);
            return this.PartialView("EmptyInventorySettings", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewInventorySettings(InventoryFieldSettingsEditViewModel model)
        {
            var inventoryTypeBusinessModel = 
                InventoryType.CreateNew(SessionFacade.CurrentCustomer.Id, model.InventoryTypeModel.Name, DateTime.Now);

            this.inventoryService.AddInventoryType(inventoryTypeBusinessModel);

            var defaultSettingsBusinessModel = 
                this.inventoryFieldsSettingsBuilder.BuildBusinessModelForAdd(inventoryTypeBusinessModel.Id, model.InventoryFieldSettingsViewModel.DefaultSettings);
            
            this.inventorySettingsService.AddInventoryFieldsSettings(defaultSettingsBusinessModel);

            this.AddDynamicFieldSetting(inventoryTypeBusinessModel.Id, model.InventoryFieldSettingsViewModel.NewDynamicFieldViewModel.InventoryDynamicFieldSettingModel);

            return this.RedirectToAction("EditSettings", new { inventoryTypeId = inventoryTypeBusinessModel.Id });
        }

        [HttpGet]
        public RedirectToRouteResult DeleteDynamicSetting(int id, int inventoryTypeId)
        {
            this.inventorySettingsService.DeleteDynamicFieldSetting(id);
            return this.RedirectToAction("InventorySettings", new { inventoryTypeId });
        }

        [HttpGet]
        public RedirectToRouteResult DeleteInventoryType(int inventoryTypeId)
        {
            this.inventoryService.DeleteInventoryType(inventoryTypeId);
            return this.RedirectToAction("Index");
        }

        private void UpdateDynamicFieldsSettings(List<InventoryDynamicFieldSettingViewModel> model)
        {
            if (model == null)
            {
                return;
            }

            var dynamicFieldsSettings =
                model.Select(
                    x =>
                    InventoryDynamicFieldSetting.CreateUpdated(
                        x.InventoryDynamicFieldSettingModel.Id,
                        x.InventoryDynamicFieldSettingModel.InventoryTypeGroupId,
                        x.InventoryDynamicFieldSettingModel.Caption,
                        x.InventoryDynamicFieldSettingModel.Position,
                        x.InventoryDynamicFieldSettingModel.FieldType.Value,
                        x.InventoryDynamicFieldSettingModel.PropertySize,
                        x.InventoryDynamicFieldSettingModel.ShowInDetails,
                        x.InventoryDynamicFieldSettingModel.ShowInList,
                        DateTime.Now,
                        x.InventoryDynamicFieldSettingModel.XMLTag,
                        x.InventoryDynamicFieldSettingModel.ReadOnly)).ToList();

            this.inventorySettingsService.UpdateDynamicFieldsSettings(dynamicFieldsSettings);
        }

        private void AddDynamicFieldSetting(
            int inventoryTypeId,
            NewInventoryDynamicFieldSettingModel newSetting)
        {

            if (string.IsNullOrWhiteSpace(newSetting.Caption) || !newSetting.FieldType.HasValue)
            {
                return;
            }

            var dynamicFieldSetting = InventoryDynamicFieldSetting.CreateNew(
                inventoryTypeId,
                newSetting.InventoryTypeGroupId,
                newSetting.Caption,
                newSetting.Position,
                newSetting.FieldType.Value,
                newSetting.PropertySize,
                newSetting.ShowInDetails,
                newSetting.ShowInList,
                DateTime.Now,
                newSetting.XMLTag,
                newSetting.ReadOnly);

            this.inventorySettingsService.AddDynamicFieldSetting(dynamicFieldSetting);
        }

        private void UpdateInventoryFieldsSettings(int inventoryTypeId, DefaultFieldSettingsModel defaultFieldSettingsModel)
        {
            var defaultSettingsBusinessModel = 
                this.inventoryFieldsSettingsBuilder.BuildBusinessModelForUpdate(inventoryTypeId, defaultFieldSettingsModel);

            this.inventorySettingsService.UpdateInventoryFieldsSettings(defaultSettingsBusinessModel);
        }

        private void UpdateInventoryType(InventoryTypeModel model)
        {
            var inventoryTypeBusinessModel = InventoryType.CreateUpdated(
                model.Name,
                model.Id,
                DateTime.Now);
            this.inventoryService.UpdateInventoryType(inventoryTypeBusinessModel);
        }
    }
}