namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports;
    using DH.Helpdesk.BusinessData.Models.Reports.Output;
    using DH.Helpdesk.Dal.Repositories;

    public sealed class ReportsService : IReportsService
    {
        private readonly IReportCustomerRepository reportCustomerRepository;

        private readonly IWorkingGroupService workingGroupService;

        private readonly ICaseTypeService caseTypeService;

        private readonly IProductAreaService productAreaService;

        private readonly ICustomerRepository customerRepository;

        public ReportsService(
            IReportCustomerRepository reportCustomerRepository, 
            IWorkingGroupService workingGroupService, 
            ICaseTypeService caseTypeService, 
            IProductAreaService productAreaService, 
            ICustomerRepository customerRepository)
        {
            this.reportCustomerRepository = reportCustomerRepository;
            this.workingGroupService = workingGroupService;
            this.caseTypeService = caseTypeService;
            this.productAreaService = productAreaService;
            this.customerRepository = customerRepository;
        }

        public SearchData GetSearchData(OperationContext context)
        {
            var reports = this.reportCustomerRepository.FindOverviews(context.CustomerId);
            var options = new SearchOptions(reports);
            return new SearchData(options);
        }

        public RegistratedCasesCaseTypeOptionsResponse GetRegistratedCasesCaseTypeOptionsResponse(OperationContext context)
        {
            var workingGroups = this.workingGroupService.GetOverviews(context.CustomerId);
            var caseTypes = this.caseTypeService.GetOverviews(context.CustomerId);
            var productAreas = this.productAreaService.GetProductAreas(context.CustomerId);

            return new RegistratedCasesCaseTypeOptionsResponse(
                                                        workingGroups,
                                                        caseTypes,
                                                        productAreas);
        }

        public RegistratedCasesCaseTypeReportResponse GetRegistratedCasesCaseTypeReportResponse(
                                                    OperationContext context,
                                                    IEnumerable<int> workingGroupsIds,
                                                    IEnumerable<int> caseTypesIds,
                                                    int? productAreaId)
        {
            var customer = this.customerRepository.GetOverview(context.CustomerId);
            var report = this.reportCustomerRepository.GetOverview(context.CustomerId, ReportType.RegistratedCasesCaseType);
            var workingGroups = this.workingGroupService.GetOverviews(context.CustomerId, workingGroupsIds);
            var caseTypes = this.caseTypeService.GetOverviews(context.CustomerId, caseTypesIds);
            var productArea = productAreaId.HasValue ? this.productAreaService.GetProductArea(productAreaId.Value) : null;

            return new RegistratedCasesCaseTypeReportResponse(
                                                        customer,
                                                        report,
                                                        workingGroups,
                                                        caseTypes,
                                                        productArea);            
        }
    }
}