namespace DH.Helpdesk.Services.Services.Concrete.Reports
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Reports.Data;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Reports;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.Services.Reports;

    public sealed class ReportService : IReportService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public ReportService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public RegistratedCasesDayOptions GetRegistratedCasesDayOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var administratorRep = uow.GetRepository<User>();

                var departments = departmentRep.GetAll().GetByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetByCustomer(customerId);
                var administrators = administratorRep.GetAll().GetByCustomer(customerId);

                return ReportsOptionsMapper.MapToRegistratedCasesDayOptions(
                                        departments,
                                        caseTypes,
                                        workingGroups,
                                        administrators);
            }
        }

        public RegistratedCasesDayData GetRegistratedCasesDayData(
            int customerId,
            int? departmentId,
            int[] caseTypeIds,
            int? workingGroupId,
            int? administratorId,
            DateTime period)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var caseRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var administratorRep = uow.GetRepository<User>();

                var from = period.RoundToMonth();
                var until = period.AddMonths(1).RoundToMonth();

                var cases = caseRep.GetAll()
                                .GetByCustomer(customerId)
                                .GetByDepartment(departmentId)
                                .GetCaseTypesCases(caseTypeIds)
                                .GetByWorkingGroup(workingGroupId)
                                .GetByAdministrator(administratorId)
                                .GetByRegistrationPeriod(from, until)
                                .GetNotDeleted();
                var departments = departmentRep.GetAll().GetByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetByCustomer(customerId);
                var administrators = administratorRep.GetAll().GetByCustomer(customerId);

                return ReportsMapper.MapToRegistratedCasesDayData(
                                cases,
                                departments,
                                caseTypes,
                                workingGroups,
                                administrators);
            }
        }

        public CaseTypeArticleNoOptions GetCaseTypeArticleNoOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var productAreaRep = uow.GetRepository<ProductArea>();

                var departments = departmentRep.GetAll().GetByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetByCustomer(customerId);
                var productAreas = productAreaRep.GetAll().GetByCustomer(customerId);

                return ReportsOptionsMapper.MapToCaseTypeArticleNoOptions(
                                departments,
                                workingGroups,
                                caseTypes,
                                productAreas);
            }
        }

        public CaseTypeArticleNoData GetCaseTypeArticleNoData(
            int customerId,
            List<int> departmentIds,
            List<int> workingGroupIds,
            List<int> caseTypeIds,
            int? productAreaId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            ShowCases showCases,
            bool isShowCaseTypeDetails,
            bool isShowPercents)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var caseRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var productAreaRep = uow.GetRepository<ProductArea>();

                var cases = caseRep.GetAll()
                                .GetByCustomer(customerId)
                                .GetByDepartments(departmentIds)
                                .GetByWorkingGroups(workingGroupIds)
                                .GetByCaseTypes(caseTypeIds)
                                .GetByProductArea(productAreaId)
                                .GetByRegistrationPeriod(periodFrom, periodUntil)
                                .GetByShowCases(showCases)
                                .GetNotDeleted();
                var departments = departmentRep.GetAll().GetByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetByCustomer(customerId);
                var productAreas = productAreaRep.GetAll().GetByCustomer(customerId);

                return ReportsMapper.MapToCaseTypeArticleNoData(
                                cases,
                                departments,
                                workingGroups,
                                caseTypes,
                                productAreas);
            }
        }
    }
}