namespace DH.Helpdesk.Web.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;

    public class InventorySettingsController : BaseController
    {
        private readonly IInventoryService inventoryService;

        private readonly IInventorySettingsService inventorySettingsService;

        public InventorySettingsController(
            IMasterDataService masterDataService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService)
            : base(masterDataService)
        {
            this.inventoryService = inventoryService;
            this.inventorySettingsService = inventorySettingsService;
        }

        [HttpGet]
        public RedirectToRouteResult RedirectToEditInventorySetttings(int currentMode)
        {
            switch ((CurrentModes)currentMode)
            {
                case CurrentModes.Workstations:
                    return this.RedirectToAction("EditWorkstationSettings");

                case CurrentModes.Servers:
                    return this.RedirectToAction("EditServerSettings");

                case CurrentModes.Printers:
                    return this.RedirectToAction("EditPrinterSettings");

                default:
                    return this.RedirectToAction("EditInventorySettings", new { inventoryTypeId = currentMode });
            }
        }

        [HttpGet]
        public ViewResult EditWorkstationSettings()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult EditServerSettings()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult EditPrinterSettings()
        {
            throw new System.NotImplementedException();
        }

        [HttpGet]
        public ViewResult EditInventorySettings(int inventorytypeid)
        {
            throw new System.NotImplementedException();
        }
    }
}