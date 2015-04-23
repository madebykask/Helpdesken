namespace DH.Helpdesk.Services.Services.Concrete.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseSatisfaction;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CasesInProgressDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ClosedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeActiveCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.LeadtimeFinishedCases;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.RegistratedCasesDay;
    using DH.Helpdesk.BusinessData.Models.Reports.Data.ReportGenerator;
    using DH.Helpdesk.BusinessData.Models.Reports.Enums;
    using DH.Helpdesk.BusinessData.Models.Reports.Options;
    using DH.Helpdesk.BusinessData.Models.Reports.Print;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Reports;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.Services.Reports;

    public sealed class ReportService : IReportService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly ISurveyService sureyService;

        public ReportService(
                IUnitOfWorkFactory unitOfWorkFactory, 
                ISurveyService sureyService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.sureyService = sureyService;
        }

        #region Reports

        public List<ReportType> GetAvailableCustomerReports(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var customerRep = uow.GetRepository<Customer>();
                var reportCustomerRep = uow.GetRepository<ReportCustomer>();

                var customers = customerRep.GetAll().GetById(customerId);
                var reports = reportCustomerRep.GetAll();

                return ReportsOptionsMapper.MapToCustomerAvailableReports(customers, reports);
            }
        }

        #endregion

        #region RegistratedCasesDay

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

        #endregion

        #region CaseTypeArticleNo

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

        #endregion

        #region ReportGenerator

        public ReportGeneratorOptions GetReportGeneratorOptions(int customerId, int languageId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var fieldRep = uow.GetRepository<CaseFieldSetting>();
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();

                var fields = fieldRep.GetAll().GetByNullableCustomer(customerId).GetShowable();
                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsOptionsMapper.MapToReportGeneratorOptions(
                                            fields,
                                            departments,
                                            workingGroups,
                                            caseTypes,
                                            languageId);
            }
        }

        public ReportGeneratorData GetReportGeneratorData(
            int customerId,
            int languageId,
            List<int> fieldIds,
            List<int> departmentIds,
            List<int> workingGroupIds,
            int? caseTypeId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            string text,
            SortField sort,
            int selectCount)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var caseRep = uow.GetRepository<Case>();
                var fieldRep = uow.GetRepository<CaseFieldSetting>();

                var settings = fieldRep.GetAll()
                            .GetByNullableCustomer(customerId)
                            .GetByIds(fieldIds)
                            .GetShowable()
                            .MapToCaseSettings(languageId);

                var caseTypeIds = new List<int>();
                if (caseTypeId.HasValue)
                {
                    LoadCaseTypeChildrenIds(caseTypeId.Value, caseTypeIds, uow);
                }

                var overviews = caseRep.GetAll().Search(
                                    customerId,
                                    departmentIds,
                                    workingGroupIds,
                                    caseTypeIds,
                                    periodFrom,
                                    periodUntil,
                                    text,
                                    sort,
                                    selectCount)
                                    .MapToCaseOverviews();

                return new ReportGeneratorData(settings, overviews);
            }
        }

        #endregion

        #region CaseSatisfaction

        public CaseSatisfactionOptionsResponse GetCaseSatisfactionOptionsResponse(OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var productAreaRep = uow.GetRepository<ProductArea>();

                var workingGroups = workingGroupRep
                                    .GetAll()
                                    .GetActiveByCustomer(context.CustomerId)
                                    .Select(g => new { g.Id, g.WorkingGroupName })
                                    .ToList()
                                    .Select(g => new ItemOverview(g.WorkingGroupName, g.Id.ToString(CultureInfo.InvariantCulture)))
                                    .ToList();

                var caseTypes = caseTypeRep
                                    .GetAll()
                                    .GetActiveByCustomer(context.CustomerId)
                                    .Select(t => new { t.Id, t.Name })
                                    .ToList()
                                    .Select(t => new ItemOverview(t.Name, t.Id.ToString(CultureInfo.InvariantCulture)))
                                    .ToList();

                var productAreas = productAreaRep
                                    .GetAll()
                                    .GetActiveByCustomer(context.CustomerId)
                                    .Select(a => new { a.Id, a.Name })
                                    .ToList()
                                    .Select(a => new ProductArea { Name = a.Name, Id = a.Id })
                                    .ToList();

                return new CaseSatisfactionOptionsResponse(workingGroups, caseTypes, productAreas);
            }
        }

        public CaseSatisfactionReportResponse GetCaseSatisfactionResponse(
            int customerId,
            DateTime finishingDateFrom,
            DateTime finishingDateTo,
            int[] selectedCaseTypes,
            int? selectedProductArea,
            int[] selectedWorkingGroups)
        {
            var res = this.sureyService.GetSurveyStat(customerId, finishingDateFrom, finishingDateTo, selectedCaseTypes, selectedProductArea, selectedWorkingGroups);
            return new CaseSatisfactionReportResponse(res.Item1, res.Item2, res.Item3, res.Item4);
        }

        public RegistratedCasesCaseTypeOptionsResponse GetRegistratedCasesCaseTypeOptionsResponse(OperationContext context)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var productAreaRep = uow.GetRepository<ProductArea>();

                var workingGroups = workingGroupRep
                                    .GetAll()
                                    .GetActiveByCustomer(context.CustomerId)
                                    .Select(g => new { g.Id, g.WorkingGroupName })
                                    .ToList()
                                    .Select(g => new ItemOverview(g.WorkingGroupName, g.Id.ToString(CultureInfo.InvariantCulture)))
                                    .ToList();

                var caseTypes = caseTypeRep
                                    .GetAll()
                                    .GetActiveByCustomer(context.CustomerId)
                                    .Select(t => new { t.Id, t.Name })
                                    .ToList()
                                    .Select(t => new ItemOverview(t.Name, t.Id.ToString(CultureInfo.InvariantCulture)))
                                    .ToList();

                var productAreas = productAreaRep
                                    .GetAll()
                                    .GetActiveByCustomer(context.CustomerId)
                                    .Select(a => new { a.Id, a.Name })
                                    .ToList()
                                    .Select(a => new ProductArea { Name = a.Name, Id = a.Id })
                                    .ToList();

                return new RegistratedCasesCaseTypeOptionsResponse(
                                                                workingGroups,
                                                                caseTypes,
                                                                productAreas);
            }           
        }

        #endregion

        #region LeadtimeFinishedCases

        public LeadtimeFinishedCasesOptions GetLeadtimeFinishedCasesOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsOptionsMapper.MapToLeadtimeFinishedCasesOptions(departments, caseTypes, workingGroups);
            }
        }

        public LeadtimeFinishedCasesData GetLeadtimeFinishedCasesData(
            int customerId,
            List<int> departmentIds,
            int? caseTypeId,
            List<int> workingGroupIds,
            GlobalEnums.RegistrationSource registrationSource,
            DateTime? periodFrom,
            DateTime? periodUntil,
            int leadTime,
            bool isShowDetails)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var casesRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();

                var cases = casesRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetByDepartments(departmentIds)
                        .GetByCaseType(caseTypeId)
                        .GetByWorkingGroups(workingGroupIds)
                        .GetByRegistrationSource(registrationSource)
                        .GetByFinishingPeriod(periodFrom, periodUntil.HasValue ? periodUntil.Value.AddMonths(1) : (DateTime?)null)
                        .GetFinished()
                        .GetNotDeleted();                
                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsMapper.MapToLeadtimeFinishedCasesData(
                            cases,
                            departments,
                            caseTypes,
                            workingGroups,
                            periodFrom,
                            periodUntil,
                            leadTime,
                            isShowDetails);
            }
        }

        #endregion

        #region LeadtimeActiveCases

        public LeadtimeActiveCasesOptions GetLeadtimeActiveCasesOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var caseTypeRep = uow.GetRepository<CaseType>();

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsOptionsMapper.MapToLeadtimeActiveCasesOptions(departments, caseTypes);
            }
        }

        public LeadtimeActiveCasesData GetLeadtimeActiveCasesData(
                        int customerId, 
                        List<int> departmentIds, 
                        int? caseTypeId,
                        int highHours,
                        int mediumDays,
                        int lowDays)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var casesRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var caseTypeRep = uow.GetRepository<CaseType>();

                var cases = casesRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetByDepartments(departmentIds)
                        .GetByCaseType(caseTypeId)
                        .HasLeadTime()
                        .GetActive()
                        .GetNotDeleted();
                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsMapper.MapToLeadtimeActiveCasesData(
                                    cases, 
                                    departments, 
                                    caseTypes,
                                    highHours,
                                    mediumDays,
                                    lowDays);
            }
        }

        #endregion

        #region FinishingCauseCustomer

        public FinishingCauseCustomerOptions GetFinishingCauseCustomerOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var administratorRep = uow.GetRepository<User>();

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);
                var administrators = administratorRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsOptionsMapper.MapToFinishingCauseCustomerOptions(
                                            departments,
                                            workingGroups,
                                            caseTypes,
                                            administrators);
            }
        }

        public FinishingCauseCustomerData GetFinishingCauseCustomerData(
            int customerId,
            List<int> departmentIds,
            int? workingGroupId,
            int? caseTypeId,
            int? administratorId,
            DateTime? periodFrom,
            DateTime? periodUntil)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var casesRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var administratorRep = uow.GetRepository<User>();
                var finishingCauseRep = uow.GetRepository<FinishingCause>();

                var caseTypeIds = new List<int>();
                if (caseTypeId.HasValue)
                {
                    LoadCaseTypeChildrenIds(caseTypeId.Value, caseTypeIds, uow);
                }

                var cases = casesRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetByDepartments(departmentIds)
                        .GetByWorkingGroup(workingGroupId)
                        .GetByCaseTypes(caseTypeIds)
                        .GetByAdministrator(administratorId)
                        .GetByRegistrationPeriod(periodFrom, periodUntil)
                        .GetActive()
                        .GetNotDeleted();

                var departments = departmentRep.GetAll().GetByIds(departmentIds);
                var workingGroups = workingGroupRep.GetAll().GetById(workingGroupId);
                var caseTypes = caseTypeRep.GetAll().GetByIds(caseTypeIds);
                var administrators = administratorRep.GetAll().GetById(administratorId);
                var finishingCauses = finishingCauseRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsMapper.MapToFinishingCauseCustomerData(
                                            cases,
                                            departments,
                                            workingGroups,
                                            caseTypes,
                                            administrators,
                                            finishingCauses,
                                            periodFrom,
                                            periodUntil);
            }
        }

        #endregion

        #region FinishingCauseCategoryCustomer

        public FinishingCauseCategoryCustomerOptions GetFinishingCauseCategoryCustomerOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsOptionsMapper.MapToFinishingCauseCategoryCustomerOptions(
                                            departments,
                                            workingGroups,
                                            caseTypes);
            }
        }

        public FinishingCauseCategoryCustomerData GetFinishingCauseCategoryCustomerData(
            int customerId,
            List<int> departmentIds,
            List<int> workingGroupIds,
            int? caseTypeId,
            DateTime? periodFrom,
            DateTime? periodUntil)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var casesRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var usersRep = uow.GetRepository<User>();

                var caseTypeIds = new List<int>();
                if (caseTypeId.HasValue)
                {
                    LoadCaseTypeChildrenIds(caseTypeId.Value, caseTypeIds, uow);
                }

                var cases = casesRep.GetAll()
                        .GetByCustomer(customerId)
                        .GetByDepartments(departmentIds)
                        .GetByWorkingGroups(workingGroupIds)
                        .GetByCaseTypes(caseTypeIds)
                        .GetByRegistrationPeriod(periodFrom, periodUntil)
                        .GetActive()
                        .GetNotDeleted();

                var departments = departmentRep.GetAll().GetByIds(departmentIds);
                var workingGroups = workingGroupRep.GetAll().GetByIds(workingGroupIds);
                var caseTypes = caseTypeRep.GetAll().GetByIds(caseTypeIds);
                var users = usersRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsMapper.MapToFinishingCauseCategoryCustomerData(
                                            cases,
                                            departments,
                                            workingGroups,
                                            caseTypes,
                                            users,
                                            periodFrom,
                                            periodUntil);
            }
        }

        #endregion

        #region ClosedCasesDay

        public ClosedCasesDayOptions GetClosedCasesDayOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var administratorRep = uow.GetRepository<User>();

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetActiveByCustomer(customerId);
                var administrators = administratorRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsOptionsMapper.MapToClosedCasesDayOptions(
                                            departments,
                                            workingGroups,
                                            caseTypes,
                                            administrators);
            }
        }

        public ClosedCasesDayData GetClosedCasesDayData(
            int customerId,
            List<int> departmentIds,
            int? workingGroupId,
            int? caseTypeId,
            int? administratorId,
            DateTime period)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var casesRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var caseTypeRep = uow.GetRepository<CaseType>();
                var administratorRep = uow.GetRepository<User>();

                var caseTypeIds = new List<int>();
                if (caseTypeId.HasValue)
                {
                    LoadCaseTypeChildrenIds(caseTypeId.Value, caseTypeIds, uow);
                }

                var from = period.RoundToMonth();
                var until = period.AddMonths(1).RoundToMonth();

                var cases = casesRep.GetAll()
                                .GetByCustomer(customerId)
                                .GetByDepartments(departmentIds)
                                .GetByCaseTypes(caseTypeIds)
                                .GetByWorkingGroup(workingGroupId)
                                .GetByAdministrator(administratorId)
                                .GetByRegistrationPeriod(from, until)
                                .GetNotDeleted();

                var departments = departmentRep.GetAll().GetByIds(departmentIds);
                var workingGroups = workingGroupRep.GetAll().GetById(workingGroupId);
                var caseTypes = caseTypeRep.GetAll().GetByIds(caseTypeIds);
                var administrators = administratorRep.GetAll().GetById(administratorId);

                return ReportsMapper.MapToClosedCasesDayData(
                                            cases,
                                            departments,
                                            workingGroups,
                                            caseTypes,
                                            administrators,
                                            period);
            }
        }

        #endregion

        #region CasesInProgressDay

        public CasesInProgressDayOptions GetCasesInProgressDayOptions(int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var administratorRep = uow.GetRepository<User>();

                var departments = departmentRep.GetAll().GetActiveByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetActiveByCustomer(customerId);
                var administrators = administratorRep.GetAll().GetActiveByCustomer(customerId);

                return ReportsOptionsMapper.MapToCasesInProgressDayOptions(
                                            departments,
                                            workingGroups,
                                            administrators);
            }
        }

        public CasesInProgressDayData GetCasesInProgressDayData(
            int customerId,
            int? departmentId,
            int? workingGroupId,
            int? administratorId,
            DateTime period)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var casesRep = uow.GetRepository<Case>();
                var departmentRep = uow.GetRepository<Department>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();
                var administratorRep = uow.GetRepository<User>();

                var from = period.RoundToMonth();
                var until = period.AddMonths(1).RoundToMonth();

                var cases = casesRep.GetAll()
                                .GetByCustomer(customerId)
                                .GetByDepartment(departmentId)
                                .GetByWorkingGroup(workingGroupId)
                                .GetByAdministrator(administratorId)
                                .GetByRegistrationPeriod(from, until)
                                .GetInProgress();

                var departments = departmentRep.GetAll().GetById(departmentId);
                var workingGroups = workingGroupRep.GetAll().GetById(workingGroupId);
                var administrators = administratorRep.GetAll().GetById(administratorId);

                return ReportsMapper.MapToCasesInProgressDayData(
                                            cases,
                                            departments,
                                            workingGroups,
                                            administrators,
                                            period);
            }
        }


        #endregion

        #region Private members

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

        private static void LoadCaseTypeChildrenIds(int id, List<int> ids, IUnitOfWork uow)
        {
            ids.Add(id);
            var children = uow
                            .GetRepository<CaseType>()
                            .GetAll()
                            .Where(a => a.Parent_CaseType_Id == id)
                            .ToList();

            foreach (var child in children)
            {
                LoadProductAreaChildrenIds(child.Id, ids, uow);
            }
        }

        #endregion
    }
}