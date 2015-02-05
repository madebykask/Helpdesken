namespace DH.Helpdesk.Services.Services.Concrete.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Reports.Print;
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

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var administrators = administratorRep.GetAll().GetActiveByCustomer(customerId);

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
                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var administrators = administratorRep.GetAll().GetActiveByCustomer(customerId);

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

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);
                var productAreas = productAreaRep.GetAll().GetActiveByCustomer(customerId);

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
            ShowCases showCases)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                return GetCaseTypeArticleNoData(
                            customerId,
                            departmentIds,
                            workingGroupIds,
                            caseTypeIds,
                            productAreaId,
                            periodFrom,
                            periodUntil,
                            showCases,
                            uow);
            }
        }

        public CaseTypeArticleNoPrintData GetCaseTypeArticleNoPrintData(
            int customerId,
            List<int> departmentIds,
            List<int> workingGroupIds,
            List<int> caseTypeIds,
            int? productAreaId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            ShowCases showCases)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var productAreaRep = uow.GetRepository<ProductArea>();

                var productAreaIds = new List<int>();
                if (productAreaId.HasValue)
                {
                    LoadProductAreaChildrenIds(productAreaId.Value, productAreaIds, uow);
                }

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId).GetByIds(departmentIds);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId).GetByIds(workingGroupIds);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId).GetByIds(caseTypeIds);
                var productAreas = productAreaRep.GetAll().GetActiveByCustomer(customerId).GetByIds(productAreaIds);
                var options = ReportsOptionsMapper.MapToCaseTypeArticleNoOptions(departments, workingGroups, caseTypes, productAreas, true);

                var data = GetCaseTypeArticleNoData(
                            customerId,
                            departmentIds,
                            workingGroupIds,
                            caseTypeIds,
                            productAreaId,
                            periodFrom, 
                            periodUntil,
                            showCases,
                            uow);

                return new CaseTypeArticleNoPrintData(
                            departmentIds != null && departmentIds.Any() ? options.Departments.Select(d => d.Name).ToList() : null,
                            workingGroupIds != null && workingGroupIds.Any() ? options.WorkingGroups.Select(g => g.Name).ToList() : null,
                            caseTypeIds != null && caseTypeIds.Any() ? options.CaseTypes.Select(t => t.Name).ToList() : null,
                            productAreaIds.Any() ? options.ProductAreas.Select(a => a.Name).ToList() : null,
                            data);
            }
        }

        private static CaseTypeArticleNoData GetCaseTypeArticleNoData(
            int customerId,
            List<int> departmentIds,
            List<int> workingGroupIds,
            List<int> caseTypeIds,
            int? productAreaId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            ShowCases showCases,
            IUnitOfWork uow)
        {
            var caseRep = uow.GetRepository<Case>();
            var departmentRep = uow.GetRepository<Department>();
            var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
            var caseTypeRep = uow.GetRepository<CaseType>();
            var productAreaRep = uow.GetRepository<ProductArea>();

            var productAreaIds = new List<int>();
            if (productAreaId.HasValue)
            {
                LoadProductAreaChildrenIds(productAreaId.Value, productAreaIds, uow);
            }

            var cases = caseRep.GetAll()
                            .GetByCustomer(customerId)
                            .GetByDepartments(departmentIds)
                            .GetByWorkingGroups(workingGroupIds)
                            .GetByCaseTypes(caseTypeIds)
                            .GetByProductAreas(productAreaIds)
                            .GetByRegistrationPeriod(periodFrom, periodUntil)
                            .GetByShowCases(showCases)
                            .GetNotDeleted();
            var departments = departmentRep.GetAll().GetActiveByCustomer(customerId).GetByIds(departmentIds);
            var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId).GetByIds(workingGroupIds);
            var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId).GetByIds(caseTypeIds);
            var productAreas = productAreaRep.GetAll().GetActiveByCustomer(customerId).GetByIds(productAreaIds);

            return ReportsMapper.MapToCaseTypeArticleNoData(
                            cases,
                            departments,
                            workingGroups,
                            caseTypes,
                            productAreas);
        }        

        private static void LoadProductAreaChildrenIds(int id, List<int> ids, IUnitOfWork uow)
        {
            ids.Add(id);
            var children = uow
                            .GetRepository<ProductArea>()
                            .GetAll()
                            .Where(a => a.Parent_ProductArea_Id == id)
                            .ToList();

            foreach (var child in children)
            {
                LoadProductAreaChildrenIds(child.Id, ids, uow);
            }
        }
    }
}