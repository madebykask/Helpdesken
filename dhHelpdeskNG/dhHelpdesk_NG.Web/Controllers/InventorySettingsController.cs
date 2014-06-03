namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory;

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
            IPrinterFieldsSettingsBuilder printerFieldsSettingsBuilder)
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
        }

        [HttpGet]
        public RedirectToRouteResult RedirectToEditInventorySetttings(int inventoryTypeId)
        {
            switch ((CurrentModes)inventoryTypeId)
            {
                case CurrentModes.Workstations:
                    return this.RedirectToAction("EditWorkstationSettings");

                case CurrentModes.Servers:
                    return this.RedirectToAction("EditServerSettings");

                case CurrentModes.Printers:
                    return this.RedirectToAction("EditPrinterSettings");

                default:
                    return this.RedirectToAction("EditInventorySettings", new { inventoryTypeId });
            }
        }

        [HttpGet]
        public ViewResult EditWorkstationSettings()
        {
            var settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsForEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            var viewModel = this.computerFieldsSettingsViewModelBuilder.BuildViewModel(settings);

            return this.View("EditWorkstationSettings", viewModel);
        }

        [HttpGet]
        public ViewResult EditServerSettings()
        {
            var settings =
                this.inventorySettingsService.GetServerFieldSettingsForEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            var viewModel = this.serverFieldsSettingsViewModelBuilder.BuildViewModel(settings);

            return this.View("EditServerSettings", viewModel);
        }

        [HttpGet]
        public ViewResult EditPrinterSettings()
        {
            var settings =
                this.inventorySettingsService.GetPrinterFieldSettingsForEdit(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            var viewModel = this.printerFieldsSettingsViewModelBuilder.BuildViewModel(settings);

            return this.View("EditPrinterSettings", viewModel);
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