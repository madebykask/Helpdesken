using System.Web.Routing;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Enums.Inventory;

    public class InventoryRoutingController : UserInteractionController
    {
        private readonly IInventoryService _inventoryService;

        public InventoryRoutingController(IMasterDataService masterDataService, IInventoryService inventoryService)
            : base(masterDataService)
        {
            _inventoryService = inventoryService;
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

        [HttpPost]
        public JsonResult GetInventoryUrlByName(string inventoryName)
        {
            var url = string.Empty;
            var id = _inventoryService.GetWorkstationIdByName(inventoryName, SessionFacade.CurrentCustomer.Id);
            if (id > 0)
            {
                url = Url.Action("Edit", "Workstation", new RouteValueDictionary(new { id, dialog = true}));
                return Json(new { success = true, url }, JsonRequestBehavior.AllowGet);
            }
            id = _inventoryService.GetServerIdByName(inventoryName, SessionFacade.CurrentCustomer.Id);
            if (id > 0)
            {
                url = Url.Action("Edit", "Server", new RouteValueDictionary(new {id, dialog = true}));
                return Json(new {success = true, url}, JsonRequestBehavior.AllowGet);
            }
            id = _inventoryService.GetPrinterIdByName(inventoryName, SessionFacade.CurrentCustomer.Id);
            if (id > 0)
            {
                url = Url.Action("Edit", "Printer", new RouteValueDictionary(new { id, dialog = true }));
                return Json(new { success = true, url }, JsonRequestBehavior.AllowGet);
            }
            var customInvTypes = _inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id);
            foreach (var customInvType in customInvTypes)
            {
                var invTypeId = int.Parse(customInvType.Value);
                id = _inventoryService.GetCustomInventoryIdByName(inventoryName, invTypeId);
                if (id > 0)
                {
                    url = Url.Action("Edit", "CustomInventory", new RouteValueDictionary(new { id, inventoryTypeId = invTypeId, dialog = true }));
                    return Json(new { success = true, url }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {success = false, url }, JsonRequestBehavior.AllowGet);
        }
    }
}