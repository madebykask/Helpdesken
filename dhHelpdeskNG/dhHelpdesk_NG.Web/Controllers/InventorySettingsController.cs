namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Printer;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server;

    public class InventorySettingsController : BaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IInventorySettingsService inventorySettingsService;

        private readonly IComputerFieldsSettingsViewModelBuilder computerFieldsSettingsViewModelBuilder;

        private readonly IServerFieldsSettingsViewModelBuilder serverFieldsSettingsViewModelBuilder;

        private readonly IPrinterFieldsSettingsViewModelBuilder printerFieldsSettingsViewModelBuilder;

        private readonly IInventoryFieldSettingsViewModelBuilder inventoryFieldSettingsViewModelBuilder;

        private readonly IComputerFieldsSettingsBuilder computerFieldsSettingsBuilder;

        private readonly IServerFieldsSettingsBuilder serverFieldsSettingsBuilder;

        private readonly IPrinterFieldsSettingsBuilder printerFieldsSettingsBuilder;

        private readonly ILanguageService languageService;

        public InventorySettingsController(
            IMasterDataService masterDataService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService,
            IComputerFieldsSettingsViewModelBuilder computerFieldsSettingsViewModelBuilder,
            IServerFieldsSettingsViewModelBuilder serverFieldsSettingsViewModelBuilder,
            IPrinterFieldsSettingsViewModelBuilder printerFieldsSettingsViewModelBuilder,
            IInventoryFieldSettingsViewModelBuilder inventoryFieldSettingsViewModelBuilder,
            IComputerFieldsSettingsBuilder computerFieldsSettingsBuilder,
            IServerFieldsSettingsBuilder serverFieldsSettingsBuilder,
            IPrinterFieldsSettingsBuilder printerFieldsSettingsBuilder,
            ILanguageService languageService)
            : base(masterDataService)
        {
            this.inventoryService = inventoryService;
            this.inventorySettingsService = inventorySettingsService;
            this.computerFieldsSettingsViewModelBuilder = computerFieldsSettingsViewModelBuilder;
            this.serverFieldsSettingsViewModelBuilder = serverFieldsSettingsViewModelBuilder;
            this.printerFieldsSettingsViewModelBuilder = printerFieldsSettingsViewModelBuilder;
            this.inventoryFieldSettingsViewModelBuilder = inventoryFieldSettingsViewModelBuilder;
            this.computerFieldsSettingsBuilder = computerFieldsSettingsBuilder;
            this.serverFieldsSettingsBuilder = serverFieldsSettingsBuilder;
            this.printerFieldsSettingsBuilder = printerFieldsSettingsBuilder;
            this.languageService = languageService;
        }

        [HttpGet]
        public ViewResult EditSettings(int inventoryTypeId)
        {
            switch ((CurrentModes)inventoryTypeId)
            {
                case CurrentModes.Workstations:
                case CurrentModes.Servers:
                case CurrentModes.Printers:
                    return this.View("EditSettings", inventoryTypeId);

                default:
                    throw new NotImplementedException();
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
                    throw new NotImplementedException();
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
                this.inventorySettingsService.GetPrinterFieldSettingsForEdit(
                    SessionFacade.CurrentCustomer.Id,
                    languageId);
            var langauges = this.languageService.GetActiveOverviews();
            var viewModel = this.printerFieldsSettingsViewModelBuilder.BuildViewModel(
                settings,
                langauges,
                languageId);

            return this.PartialView("PrinterSettings", viewModel);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public void PrinterSettings(PrinterFieldsSettingsViewModel model)
        {
            var businessModel = this.printerFieldsSettingsBuilder.BuildViewModel(
                model,
                SessionFacade.CurrentCustomer.Id);

            this.inventorySettingsService.UpdatePrinterFieldsSettings(businessModel);
        }

        [HttpGet]
        public ViewResult EditInventorySettings(int inventoryTypeid)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult DeleteDynamicSetting(int id)
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult NewCustomInventoryType()
        {
            throw new System.NotImplementedException();
        }
    }
}