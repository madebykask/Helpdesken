namespace DH.Helpdesk.Services.Services.Concrete
{
    using System;

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

        private readonly ICaseService caseService;

        private readonly IDepartmentService departmentService;

        private readonly IUserService userService;

        public ReportsService(
            IReportCustomerRepository reportCustomerRepository, 
            IWorkingGroupService workingGroupService, 
            ICaseTypeService caseTypeService, 
            IProductAreaService productAreaService, 
            ICustomerRepository customerRepository, 
            ICaseService caseService, 
            IDepartmentService departmentService, 
            IUserService userService)
        {
            this.reportCustomerRepository = reportCustomerRepository;
            this.workingGroupService = workingGroupService;
            this.caseTypeService = caseTypeService;
            this.productAreaService = productAreaService;
            this.customerRepository = customerRepository;
            this.caseService = caseService;
            this.departmentService = departmentService;
            this.userService = userService;
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
                                                    int[] workingGroupsIds,
                                                    int[] caseTypesIds,
                                                    int? productAreaId,
                                                    DateTime periodFrom,
                                                    DateTime periodUntil)
        {
            var customer = this.customerRepository.GetOverview(context.CustomerId);
            var report = this.reportCustomerRepository.GetOverview(context.CustomerId, ReportType.RegistratedCasesCaseType);
            var workingGroups = this.workingGroupService.GetOverviews(context.CustomerId, workingGroupsIds);
            var caseTypes = this.caseTypeService.GetOverviews(context.CustomerId, caseTypesIds);
            var productArea = productAreaId.HasValue ? this.productAreaService.GetProductArea(productAreaId.Value) : null;
            var items = this.caseService.GetRegistratedCasesCaseTypeItems(
                                            context.CustomerId,
                                            workingGroupsIds,
                                            caseTypesIds,
                                            productAreaId,
                                            periodFrom,
                                            periodUntil);
            return new RegistratedCasesCaseTypeReportResponse(
                                                        customer,
                                                        report,
                                                        workingGroups,
                                                        caseTypes,
                                                        productArea,
                                                        items);            
        }

        public RegistratedCasesDayOptionsResponse GetRegistratedCasesDayOptionsResponse(OperationContext context)
        {
            var departments = this.departmentService.FindActiveOverviews(context.CustomerId);
            var caseTypes = this.caseTypeService.GetOverviews(context.CustomerId);
            var workingGroups = this.workingGroupService.GetOverviews(context.CustomerId);
            var administrators = this.userService.FindActiveOverviews(context.CustomerId);

            return new RegistratedCasesDayOptionsResponse(
                                                        departments,
                                                        caseTypes,
                                                        workingGroups,
                                                        administrators);
        }

        public RegistratedCasesDayReportResponse GetRegistratedCasesDayReportResponse(
                                            OperationContext context,
                                            int? departmentId,
                                            int[] caseTypesIds,
                                            int? workingGroupId,
                                            int? administrator,
                                            DateTime period)
        {
//            var customer = this.customerRepository.GetOverview(context.CustomerId);
//            var report = this.reportCustomerRepository.GetOverview(context.CustomerId, ReportType.RegistratedCasesDay);
//
//            return new RegistratedCasesDayReportResponse(
//                                            customer,
//                                            report,
//                                            );
            return null;
        }
    }
}