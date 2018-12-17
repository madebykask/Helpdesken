namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.DisplayValues;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Areas.Inventory.Models;
    using DH.Helpdesk.Web.Areas.Inventory.Models.SearchModels;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;

    public class CustomInventoryReportController : ComputerInventoryBaseController
    {
        private readonly IInventoryService inventoryService;

        public CustomInventoryReportController(
            IMasterDataService masterDataService,
            IExportFileNameFormatter exportFileNameFormatter,
            IExcelFileComposer excelFileComposer,
            IOrganizationService organizationService,
            IPlaceService placeService,
            IInventoryService inventoryService,
            IInventorySettingsService inventorySettingsService,
            IComputerModulesService computerModulesService)
            : base(
                masterDataService,
                exportFileNameFormatter,
                excelFileComposer,
                organizationService,
                placeService,
                inventoryService,
                inventorySettingsService,
                computerModulesService)
        {
            this.inventoryService = inventoryService;
        }

        [HttpGet]
        public ViewResult Index(int inventoryTypeId)
        {
            List<ItemOverview> inventoryTypes = this.inventoryService.GetInventoryTypes(
                SessionFacade.CurrentCustomer.Id);

            SessionFacade.SavePageFilters(TabName.Reports, new ReportFilter(inventoryTypeId));
            var currentFilter =
                SessionFacade.FindPageFilters<CustomTypeReportsSearchFilter>(CustomTypeReportsSearchFilter.CreateFilterId()) ?? 
                CustomTypeReportsSearchFilter.CreateDefault();

            var departments = this.OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);

            var viewModel = CustomTypeReportSearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                inventoryTypeId,
                inventoryTypeId,
                inventoryTypes);

            return this.View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult CustomInventoryReportGrid(CustomTypeReportsSearchFilter filter, int inventoryTypeId)
        {
            SessionFacade.SavePageFilters(CustomTypeReportsSearchFilter.CreateFilterId(), filter);

            ReportModelWithInventoryType models = this.inventoryService.GetAllConnectedInventory(
                inventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor);

            List<ReportModelWrapper> wrapReportModels = ReportModelWrapper.Wrap(
                models.ReportModel,
                ReportModelWrapper.ReportOwnerTypes.Workstation);

            var viewModel = new ReportViewModel(wrapReportModels, models.InventoryName, filter.IsShowParentInventory);

            return this.PartialView("CustomInventoryReportGrid", viewModel);
        }

        [HttpGet]
        public FileContentResult ExportReportToExcelFile(int inventoryTypeId)
        {
            return this.CreateCustomTypeReport(inventoryTypeId);
        }

        private FileContentResult CreateCustomTypeReport(int inventoryTypeId)
        {
            var filter =
                SessionFacade.FindPageFilters<CustomTypeReportsSearchFilter>(CustomTypeReportsSearchFilter.CreateFilterId()) ?? 
                CustomTypeReportsSearchFilter.CreateDefault();

            var models = this.inventoryService.GetAllConnectedInventory(
                inventoryTypeId,
                filter.DepartmentId,
                filter.SearchFor);

            return this.CreateCustomTypeReport(models, filter.IsShowParentInventory);
        }

        private FileContentResult CreateCustomTypeReport(ReportModelWithInventoryType viewModel, bool isGrouped)
        {
            const string NameCellFieldName = "Name";

            var headers = new List<ExcelTableHeader>
                              {
                                  new ExcelTableHeader(viewModel.InventoryName, NameCellFieldName)
                              };

            var source = new List<BusinessItem>();

            IEnumerable<IGrouping<string, ReportModel>> items =
                viewModel.ReportModel.OrderBy(x => x.Item).ThenBy(x => x.Owner).GroupBy(x => x.Item);

            foreach (var item in items)
            {
                var nameCell = new BusinessItemField(NameCellFieldName, new StringDisplayValue(item.Key))
                                   {
                                       IsBold =
                                           isGrouped
                                   };

                var cells = new List<BusinessItemField> { nameCell };
                var businessItem = new BusinessItem(cells);
                source.Add(businessItem);

                if (isGrouped)
                {
                    source.AddRange(
                        from owner in item.Where(x => x.Owner != null)
                        select new BusinessItemField(NameCellFieldName, new StringDisplayValue(owner.Owner))
                        into ownerCell
                        select new List<BusinessItemField> { ownerCell }
                        into ownerCells
                        select new BusinessItem(ownerCells));
                }
            }

            var file = this.CreateExcelReport(viewModel.InventoryName, headers, source);

            return file;
        }
    }
}