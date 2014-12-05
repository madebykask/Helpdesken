namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer;
    using DH.Helpdesk.Web.Infrastructure;

    public class ComputerInventoryBaseController : InventoryBaseController
    {
        protected readonly IComputerModulesService ComputerModulesService;

        private readonly IInventoryService inventoryService;

        private readonly IInventorySettingsService inventorySettingsService;

        public ComputerInventoryBaseController(
            IMasterDataService masterDataService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService,
            IComputerModulesService computerModulesService)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this.inventoryService = inventoryService;
            this.inventorySettingsService = inventorySettingsService;
            this.ComputerModulesService = computerModulesService;
        }

        [HttpGet]
        public PartialViewResult SearchComputerShortInfo(int computerId)
        {
            var model = this.inventoryService.GetWorkstationShortInfo(computerId);
            var settings =
                this.inventorySettingsService.GetWorkstationFieldSettingsForShortInfo(
                    SessionFacade.CurrentCustomer.Id,
                    SessionFacade.CurrentLanguageId);
            var softwares = this.ComputerModulesService.GetComputerSoftware(computerId);
            var drives = this.ComputerModulesService.GetComputerLogicalDrive(computerId);
            var logs = this.inventoryService.GetWorkstationLogOverviews(computerId);

            var viewModel = new ComputerModalViewModel(model, settings, softwares, drives, logs);

            return this.PartialView("ComputerShortInfoDialog", viewModel);
        }

        [HttpPost]
        public void EditComputerInfo(int id, string info)
        {
            this.inventoryService.UpdateWorkstationInfo(id, info);
        }
    }
}