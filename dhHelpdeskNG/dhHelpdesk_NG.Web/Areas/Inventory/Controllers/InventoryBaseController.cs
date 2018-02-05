using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Web.Infrastructure.CaseOverview;
using DH.Helpdesk.Web.Infrastructure.ModelFactories.Case;
using DH.Helpdesk.Web.Models.Case;

namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Controllers;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;

    public class InventoryBaseController : UserInteractionController
    {
        protected readonly IExportFileNameFormatter ExportFileNameFormatter;

        protected readonly IExcelFileComposer ExcelFileComposer;

        protected readonly IOrganizationService OrganizationService;

        protected readonly IPlaceService PlaceService;

        public InventoryBaseController(
            IMasterDataService masterDataService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            IOrganizationService organizationService,
            IPlaceService placeService)
            : base(masterDataService)
        {
            this.ExportFileNameFormatter = exportFileNameFormatter;
            this.ExcelFileComposer = excelFileComposer;
            this.OrganizationService = organizationService;
            this.PlaceService = placeService;
        }

        [HttpGet]
        public JsonResult SearchDepartmentsByRegionId(int? selected)
        {
            List<ItemOverview> models = this.OrganizationService.GetDepartments(
                SessionFacade.CurrentCustomer.Id,
                selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchFloorsByBuildingId(int? selected)
        {
            List<ItemOverview> models = this.PlaceService.GetFloors(SessionFacade.CurrentCustomer.Id, selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchRoomsByFloorId(int? selected)
        {
            List<ItemOverview> models = this.PlaceService.GetRooms(SessionFacade.CurrentCustomer.Id, selected);
            return this.Json(models, JsonRequestBehavior.AllowGet);
        }

        protected FileContentResult CreateExcelReport(
           string worksheetName,
           IEnumerable<ITableHeader> headers,
           IEnumerable<IRow<ICell>> rows)
        {
            const string Name = "Inventory";

            byte[] content = this.ExcelFileComposer.Compose(headers, rows, worksheetName);

            string fileName = this.ExportFileNameFormatter.Format(Name, "xlsx");
            return this.File(content, MimeType.ExcelFile, fileName);
        }

        protected string CreateFilterId(string tabName, string id)
        {
            return string.Format("{0}{1}", tabName, id);
        }
    }
}