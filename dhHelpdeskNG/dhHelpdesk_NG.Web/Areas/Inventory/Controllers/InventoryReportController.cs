namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;

    public class InventoryReportController : InventoryBaseController
    {
        private readonly IInventoryService inventoryService;

        public InventoryReportController(
            IMasterDataService masterDataService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IInventoryService inventoryService)
            : base(masterDataService, exportFileNameFormatter, excelFileComposer, organizationService, placeService)
        {
            this.inventoryService = inventoryService;
        }

        [HttpGet]
        public ViewResult Index()
        {
            var inventoryTypes = inventoryService.GetInventoryTypes(SessionFacade.CurrentCustomer.Id);

            SessionFacade.SavePageFilters(TabName.Reports, new ReportFilter((int)ReportTypes.Inventory));

            var currentFilter =
                SessionFacade.FindPageFilters<InventoryReportSearchFilter>(
                    InventoryReportSearchFilter.CreateFilterId()) ?? InventoryReportSearchFilter.CreateDefault();

            var departments = OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);

            var viewModel = 
                InventoryReportSearchViewModel.BuildViewModel(
                    currentFilter,
                    departments,
                    (int)ReportTypes.Inventory,
                    inventoryTypes);

            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult InventoryReportGrid(InventoryReportSearchFilter filter)
        {
            SessionFacade.SavePageFilters(InventoryReportSearchFilter.CreateFilterId(), filter);

            var models =
                this.inventoryService.GetInventoryCounts(SessionFacade.CurrentCustomer.Id, filter.DepartmentId);

            return this.PartialView("InventoryReportGrid", models);
        }

        [HttpGet]
        public FileContentResult ExportReportToExcelFile()
        {
            return this.CreateInventoryReport();
        }

        private FileContentResult CreateInventoryReport()
        {
            var filter =
                SessionFacade.FindPageFilters<InventoryReportSearchFilter>(
                    InventoryReportSearchFilter.CreateFilterId()) ?? InventoryReportSearchFilter.CreateDefault();

            List<InventoryReportModel> models =
                this.inventoryService.GetInventoryCounts(SessionFacade.CurrentCustomer.Id, filter.DepartmentId);

            return this.CreateInventoryReport(models);
        }

        private FileContentResult CreateInventoryReport(List<InventoryReportModel> models)
        {
            const string NameCellFieldName = "Name";
            const string CountCellFieldName = "Count";

            var headers = new List<ExcelTableHeader>
                              {
                                  new ExcelTableHeader(Translation.Get("Inventarie"), NameCellFieldName),
                                  new ExcelTableHeader(Translation.Get("Antal"), CountCellFieldName)
                              };

            var source = (from model in models
                          let nameCell = new BusinessItemField(NameCellFieldName, new StringDisplayValue(model.InventoryName))
                          let countCell = new BusinessItemField(CountCellFieldName, new IntegerDisplayValue(model.Count))
                          select new List<BusinessItemField> { nameCell, countCell } into cells
                          select new BusinessItem(cells)).ToList();

            var file = this.CreateExcelReport(Translation.Get("Inventarie"), headers, source);
            return file;
        }
    }
}