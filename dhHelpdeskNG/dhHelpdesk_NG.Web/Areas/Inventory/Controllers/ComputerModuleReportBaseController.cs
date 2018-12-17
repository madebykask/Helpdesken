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

    public abstract class ComputerModuleReportBaseController : ComputerInventoryBaseController
    {
        private readonly IInventoryService inventoryService;

        protected ComputerModuleReportBaseController(
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

        public abstract ReportTypes ReportType { get; }

        [HttpGet]
        public ViewResult Index()
        {
            List<ItemOverview> inventoryTypes = this.inventoryService.GetInventoryTypes(
                SessionFacade.CurrentCustomer.Id);

            SessionFacade.SavePageFilters(TabName.Reports, new ReportFilter((int)this.ReportType));
            var currentFilter =
                SessionFacade.FindPageFilters<ReportsSearchFilter>(ReportsSearchFilter.CreateFilterId()) ?? 
                ReportsSearchFilter.CreateDefault();

            List<ItemOverview> departments = this.OrganizationService.GetDepartments(SessionFacade.CurrentCustomer.Id);

            ReportsSearchViewModel viewModel = ReportsSearchViewModel.BuildViewModel(
                currentFilter,
                departments,
                (int)this.ReportType,
                inventoryTypes);

            return this.View("IndexReport", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult ReportsGrid(ReportsSearchFilter filter)
        {
            SessionFacade.SavePageFilters(ReportsSearchFilter.CreateFilterId(),filter);

            ReportViewModel viewModel = this.BuildViewModel(
                filter.ReportDataType,
                filter.DepartmentId,
                filter.SearchFor,
                filter.IsShowParentInventory);

            return this.PartialView("ReportGrid", viewModel);
        }

        [HttpGet]
        public FileContentResult ExportReportToExcelFile()
        {
            return this.CreateModuleReport();
        }

        protected ReportViewModel BuildViewModel(
            ReportDataTypes reportDataType,
            int? departmentId,
            string searchFor,
            bool isShowParentInventory)
        {
            var wrapModels = new List<ReportModelWrapper>();

            switch (reportDataType)
            {
                case ReportDataTypes.Workstation:
                    wrapModels = this.CreateWorkstationReportModelWrappers(departmentId, searchFor);
                    break;
                case ReportDataTypes.Server:
                    wrapModels = this.CreateServerReportModelWrappers(searchFor);
                    break;
                case ReportDataTypes.All:
                    var computerSoftware = this.CreateWorkstationReportModelWrappers(departmentId, searchFor);
                    var serverSoftware = this.CreateServerReportModelWrappers(searchFor);
                    wrapModels = computerSoftware.Union(serverSoftware).ToList();
                    break;
            }

            var viewModel = new ReportViewModel(wrapModels, this.GetHeader(), isShowParentInventory);

            return viewModel;
        }

        protected abstract List<ReportModel> GetComputerReportModels(int? departmentId, string searchFor);

        protected abstract List<ReportModel> GetServerReportModels(string searchFor);

        protected abstract string GetHeader();

        private List<ReportModelWrapper> CreateWorkstationReportModelWrappers(int? departmentId, string searchFor)
        {
            var computerModels = this.GetComputerReportModels(departmentId, searchFor);
            var wrapModels = ReportModelWrapper.Wrap(computerModels, ReportModelWrapper.ReportOwnerTypes.Workstation);
            return wrapModels;
        }

        private List<ReportModelWrapper> CreateServerReportModelWrappers(string searchFor)
        {
            var serverModels = this.GetServerReportModels(searchFor);
            var wrapModels = ReportModelWrapper.Wrap(serverModels, ReportModelWrapper.ReportOwnerTypes.Server);
            return wrapModels;
        }

        private FileContentResult CreateModuleReport()
        {
            var filter =
                SessionFacade.FindPageFilters<ReportsSearchFilter>(ReportsSearchFilter.CreateFilterId()) ?? 
                ReportsSearchFilter.CreateDefault();

            ReportViewModel viewModel = this.BuildViewModel(
                filter.ReportDataType,
                filter.DepartmentId,
                filter.SearchFor,
                filter.IsShowParentInventory);

            return this.CreateReportGridReport(viewModel);
        }

        private FileContentResult CreateReportGridReport(ReportViewModel viewModel)
        {
            const string NameCellFieldName = "Name";
            const string CountCellFieldName = "Count";

            var headers = new List<ExcelTableHeader>
                              {
                                  new ExcelTableHeader(viewModel.Header, NameCellFieldName),
                                  new ExcelTableHeader(
                                      Translation.Get("Antal"),
                                      CountCellFieldName)
                              };

            var source = new List<BusinessItem>();

            IEnumerable<IGrouping<string, ReportModelWrapper>> items =
                viewModel.ReportModel.OrderBy(x => x.ReportModel.Item)
                    .ThenBy(x => x.ReportModel.Owner)
                    .GroupBy(x => x.ReportModel.Item);

            foreach (var item in items)
            {
                var nameCell = new BusinessItemField(NameCellFieldName, new StringDisplayValue(item.Key))
                                   {
                                       IsBold =
                                           viewModel
                                           .IsGrouped
                                   };
                var countCell = new BusinessItemField(CountCellFieldName, new IntegerDisplayValue(item.Count()))
                                    {
                                        IsBold
                                            =
                                            viewModel
                                            .IsGrouped
                                    };

                var cells = new List<BusinessItemField> { nameCell, countCell };
                var businessItem = new BusinessItem(cells);
                source.Add(businessItem);

                if (viewModel.IsGrouped)
                {
                    source.AddRange(
                        from owner in item.Where(x => x.ReportModel.Owner != null)
                        select new BusinessItemField(NameCellFieldName, new StringDisplayValue(owner.ReportModel.Owner))
                        into ownerCell
                        select new List<BusinessItemField> { ownerCell }
                        into ownerCells
                        select new BusinessItem(ownerCells));
                }
            }

            var file = this.CreateExcelReport(Translation.Get(viewModel.Header), headers, source);

            return file;
        }
    }
}