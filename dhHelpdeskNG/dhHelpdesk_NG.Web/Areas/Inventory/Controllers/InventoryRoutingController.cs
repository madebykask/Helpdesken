namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Enums.Inventory;

    public class InventoryRoutingController : UserInteractionController
    {
        public InventoryRoutingController(IMasterDataService masterDataService)
            : base(masterDataService)
        {
        }

        [HttpPost]
        public RedirectToRouteResult RedirectToIndex(int currentMode)
        {
            switch ((CurrentModes)currentMode)
            {
                case CurrentModes.Workstations:
                    return this.RedirectToAction("Index", "Workstation");

                case CurrentModes.Servers:
                    return this.RedirectToAction("Index", "Server");

                case CurrentModes.Printers:
                    return this.RedirectToAction("Index", "Printer");

                default:
                    return this.RedirectToAction("Index", "CustomInventory", new { inventoryTypeId = currentMode });
            }
        }

        [HttpPost]
        public RedirectToRouteResult RedirectToReportIndex(int reportType)
        {
            switch ((ReportTypes)reportType)
            {
                case ReportTypes.OperatingSystem:
                    return this.RedirectToAction("Index", "OperatingSystemReport");

                case ReportTypes.ServicePack:
                    return this.RedirectToAction("Index", "ServicePackReport");

                case ReportTypes.Processor:
                    return this.RedirectToAction("Index", "ProcessorReport");

                case ReportTypes.Ram:
                    return this.RedirectToAction("Index", "RamReport");

                case ReportTypes.NetworkAdapter:
                    return this.RedirectToAction("Index", "NetworkAdapterReport");

                case ReportTypes.Location:
                    return this.RedirectToAction("Index", "LocationReport");

                case ReportTypes.InstaledPrograms:
                    return this.RedirectToAction("Index", "InstaledProgramReport");

                case ReportTypes.Inventory:
                    return this.RedirectToAction("Index", "InventoryReport");

                default:
                    return this.RedirectToAction("Index", "CustomInventoryReport", new { inventoryTypeId = reportType });
            }
        }

        [HttpPost]
        public RedirectToRouteResult RedirectToMasterDataIndex(ModuleTypes moduleType)
        {
            return this.RedirectToAction("Index", moduleType.ToString());
        }

        [HttpGet]
        public RedirectToRouteResult RedirectToEditInventory(int id, int inventoryTypeId)
        {
            return this.RedirectToAction("Edit", "CustomInventory");
        }
    }
}