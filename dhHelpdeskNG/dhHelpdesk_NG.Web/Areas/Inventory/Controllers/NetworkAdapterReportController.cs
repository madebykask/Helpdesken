namespace DH.Helpdesk.Web.Areas.Inventory.Controllers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport;
    using DH.Helpdesk.Services.BusinessLogic.BusinessModelExport.ExcelExport;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Enums.Inventory;
    using DH.Helpdesk.Web.Infrastructure;

    public class NetworkAdapterReportController : ComputerModuleReportBaseController
    {
        public NetworkAdapterReportController(
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
        }

        public override ReportTypes ReportType
        {
            get
            {
                return ReportTypes.NetworkAdapter;
            }
        }

        protected override List<ReportModel> GetComputerReportModels(int? departmentId, string searchFor)
        {
            return this.ComputerModulesService.GetConnectedToComputersNicOverviews(
                            SessionFacade.CurrentCustomer.Id,
                            departmentId,
                            searchFor);
        }

        protected override List<ReportModel> GetServerReportModels(string searchFor)
        {
            return this.ComputerModulesService.GetConnectedToServersNicOverviews(
                                        SessionFacade.CurrentCustomer.Id,
                                        searchFor);
        }

        protected override string GetHeader()
        {
            return "Nätverkskort";
        }
    }
}