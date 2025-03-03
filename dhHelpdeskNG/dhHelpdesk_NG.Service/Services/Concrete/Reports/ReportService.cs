using DH.Helpdesk.BusinessData.Models.Reports;

namespace DH.Helpdesk.Services.Services.Concrete.Reports
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	using DH.Helpdesk.BusinessData.Models;
	using DH.Helpdesk.BusinessData.Models.Grid;
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
	using DH.Helpdesk.Domain.Cases;
	using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;
	using DH.Helpdesk.Services.BusinessLogic.Mappers.Reports;
	using DH.Helpdesk.Services.BusinessLogic.Specifications;
	using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
	using DH.Helpdesk.Services.Services.Reports;
	using DH.Helpdesk.Common.Enums;
	using DH.Helpdesk.Dal.Repositories;
	using DH.Helpdesk.BusinessData.Enums.Case.Fields;
	using DH.Helpdesk.BusinessData.Models.Case.CaseOverview;
	using BusinessData.Models.Case.CaseSettingsOverview;
    using BusinessData.Models.Case;

	public sealed class ReportService : IReportService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;        
        private readonly ISurveyService sureyService;
        private readonly IReportRepository _reportRepository;
        private readonly IReportFavoriteRepository _reportFavoriteRepository;
        private readonly IDepartmentService _departmentService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IUserService _userService;
        private readonly IProductAreaRepository _productAreaRepository;
        private readonly ICaseTypeRepository _caseTypeRepository;
		private readonly IFeatureToggleService _featureToggleService;
		private readonly IExtendedCaseService _extendedCaseService;
        private readonly IOUService _oUService;

        public ReportService(
                IUnitOfWorkFactory unitOfWorkFactory,
                ISurveyService sureyService,
                IReportRepository reportRepository,
                IReportFavoriteRepository reportFavoriteRepository,
                IDepartmentService departmentService,
                IWorkingGroupService workingGroupService,
                IUserService userService,
                IProductAreaRepository productAreaRepository,
                ICaseTypeRepository caseTypeRepository,
				IFeatureToggleService featureToggleService,
				IExtendedCaseService extendedCaseService,
                IOUService oUService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.sureyService = sureyService;
            this._reportRepository = reportRepository;
            this._reportFavoriteRepository = reportFavoriteRepository;
            this._departmentService = departmentService;
            this._workingGroupService = workingGroupService;
            this._userService = userService;
            _productAreaRepository = productAreaRepository;
            _caseTypeRepository = caseTypeRepository;
			_featureToggleService = featureToggleService;
			_extendedCaseService = extendedCaseService;
            _oUService = oUService;

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

        #region ReportFavorites

        public List<ReportFavoriteList> GetCustomerReportFavoriteList(int customerId, int? userId)
        {
            var reports = _reportFavoriteRepository.GetCustomerReportFavoriteList(customerId, userId);

            return reports.ToList();
        }

        public ReportFavorite GetCustomerReportFavorite(int reportFavoriteId, int customerId, int? userId)
        {
            var report = _reportFavoriteRepository.GetCustomerReportFavoriteById(reportFavoriteId, customerId, userId);

            return report;
        }

        public int SaveCustomerReportFavorite(ReportFavorite reportFavorite)
        {
            if (!_reportFavoriteRepository.IsCustomerReportFavoriteNameUnique(reportFavorite.Id, reportFavorite.Customer_Id, reportFavorite.User_Id, reportFavorite.Name))
            {
                return -2;
            }

            reportFavorite.UpdateDate = DateTime.UtcNow;

            if (reportFavorite.IsNew())
            {
                _reportFavoriteRepository.Add(reportFavorite);
            }
            else
            {
                var report = _reportFavoriteRepository.GetCustomerReportFavoriteById(reportFavorite.Id, reportFavorite.Customer_Id, reportFavorite.User_Id);

                if (report == null)
                {
                    return -1;
                }

                report.Filters = reportFavorite.Filters;
                report.Name = reportFavorite.Name;
                report.Type = reportFavorite.Type;
            }

            _reportFavoriteRepository.Commit();

            return reportFavorite.Id;
        }

        public void DeleteCustomerReportFavorite(int reportFavoriteId, int customerId, int? userId)
        {
            _reportFavoriteRepository.DeleteCustomerReportFavoriteById(reportFavoriteId, customerId, userId);
            _reportFavoriteRepository.Commit();
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
                var fields = fieldRep.GetAll()
                    .GetByNullableCustomer(customerId)
                    .GetShowable()
                    .Where(it => !GridColumnsDefinition.NotAvailableField.Contains(it.Name));
                var departments = departmentRep.GetAll().GetByCustomer(customerId);
                var workingGroups = workingGroupRep.GetAll().GetByCustomer(customerId);
                var caseTypes = caseTypeRep.GetAll().GetByCustomer(customerId);

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
            int userId,
            int languageId,
            List<int> fieldIds,            
            List<int> departmentIds,
            List<int> ouIds,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeIds,
			List<string> extendedCaseFormFieldIds,
			int? extendedCaseFormId,
            DateTime? periodFrom,
            DateTime? periodUntil,
            string text,
            SortField sort,
            int selectCount,
            DateTime? closeFrom,
            DateTime? closeTo)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {

                List<Tuple<int, int>> departmentAndOuis = new List<Tuple<int, int>>();

                //Populate the departments
                foreach (var department in departmentIds)
                {
                    departmentAndOuis.Add(new Tuple<int, int>(department, 0));
                }


                //Populate the departments + OUs
                foreach (var ou in ouIds)
                {
                    var ouEntity = _oUService.GetOU(ou);

                    if (ouEntity.Department_Id.HasValue)
                    {
                        departmentAndOuis.Add(new Tuple<int, int>(ouEntity.Department_Id.Value, ou));
                    }
                }

                uow.AutoDetectChangesEnabled = false;
                var categoryRep = uow.GetRepository<Category>();
                var ouRep = uow.GetRepository<OU>();
                var finishingCauseRep = uow.GetRepository<FinishingCause>();
                var fieldRep = uow.GetRepository<CaseFieldSetting>();
                var manualFields = new List<string>();
                if (fieldIds.Count == 0)
                {
                    manualFields.Add(CaseInfoFields.LeadTime);
                    manualFields.Add(LogFields.TotalWork);
                    manualFields.Add(LogFields.TotalOverTime);
                    manualFields.Add(LogFields.TotalMaterial);
                    manualFields.Add(LogFields.TotalPrice);
                }
                else
                {
                    if (fieldIds.Contains(Convert.ToInt32(CalculationFields.LeadTime)))
                        manualFields.Add(CaseInfoFields.LeadTime);
                    if (fieldIds.Contains(Convert.ToInt32(CalculationFields.TotalWork)))
                        manualFields.Add(LogFields.TotalWork);
                    if (fieldIds.Contains(Convert.ToInt32(CalculationFields.TotalOverTime)))
                        manualFields.Add(LogFields.TotalOverTime);
                    if (fieldIds.Contains(Convert.ToInt32(CalculationFields.TotalMaterial)))
                        manualFields.Add(LogFields.TotalMaterial);
                    if (fieldIds.Contains(Convert.ToInt32(CalculationFields.TotalPrice)))
                        manualFields.Add(LogFields.TotalPrice);
                }

                var settings = fieldRep.GetAll()
                            .GetByNullableCustomer(customerId)
                            .GetByIds(fieldIds)
                            .GetShowable()
                            .MapToCaseSettings(languageId, manualFields);

				

                var caseTypeChainIds = new List<int>();
                foreach (var caseTypeId in caseTypeIds)
                {
                    LoadCaseTypeChildrenIds(caseTypeId, caseTypeChainIds, uow);
                }

                var productAreaChainIds = new List<int>();
                foreach (var productAreaId in productAreaIds)
                {
                    LoadProductAreaChildrenIds(productAreaId, productAreaChainIds, uow);
                }

                var categories = categoryRep.GetAll().GetByCustomer(customerId);
                var ous = ouRep.GetAll();
                var finishingCauses = finishingCauseRep.GetAll().GetByCustomer(customerId);

                //Ensure filters
                departmentAndOuis = EnsureDepartments(departmentAndOuis, userId, customerId);
                workingGroupIds = EnsureWorkingGroups(workingGroupIds, userId, customerId);

				var usePreviousSearchToggle = _featureToggleService.Get(Common.Constants.FeatureToggleTypes.REPORTS_REPORTGENERATOR_USE_PREVIOUS_SEARCH);

				List<ReportGeneratorFields> caseData;
				if (usePreviousSearchToggle.Active)
				{
					caseData = _reportRepository.GetCaseList_FeatureTooglePreviousSearch(
												   customerId,
                                                   departmentAndOuis,
                                                   workingGroupIds,
												   productAreaChainIds,
												   administratorIds,
												   caseStatusIds,
												   caseTypeChainIds,
												   periodFrom,
												   periodUntil,
												   closeFrom,
												   closeTo);
				}
				else
				{
					caseData = _reportRepository.GetCaseList(
												   customerId,
                                                   departmentAndOuis,
                                                   workingGroupIds,
												   productAreaChainIds,
												   administratorIds,
												   caseStatusIds,
												   caseTypeChainIds,
												   extendedCaseFormFieldIds,
												   extendedCaseFormId,
												   periodFrom,
												   periodUntil,
												   closeFrom,
												   closeTo);
				}
                
                var overviews = caseData.MapToCaseOverviews(_caseTypeRepository, _productAreaRepository, ous, finishingCauses, categories);

                var sortedOverviews = Sort(overviews, sort);

				if (extendedCaseFormId.HasValue)
				{
					var fields = _extendedCaseService.GetExtendedCaseFormFields(extendedCaseFormId.Value, languageId).ToDictionary(o => o.FieldId);
                    var sections = _extendedCaseService.GetExtendedCaseFormSections(extendedCaseFormId.Value, languageId).ToDictionary(o => o.SectionId);                    

					if (extendedCaseFormFieldIds.Any())
					{
                        settings.ExtendedCase = new ExtendedCaseSettings
                        {
                            Fields = extendedCaseFormFieldIds
                                .Select(o => new ExtendedCaseField
                                {
                                    FieldId = o,
                                    Name = sections[o].Text + " - " + fields[o].Text
                                }).OrderBy(s => s.Name).ToList(),

                            Sections = extendedCaseFormFieldIds
                                .Select(o => new ExtendedCaseSection
                                {
                                    SectionId = o,
                                    Name = sections[o].Text
                                }).ToList()
                        };
					}
					else
					{
						settings.ExtendedCase = new ExtendedCaseSettings
						{
							Fields = fields
								.Select(o => new ExtendedCaseField
								{
									FieldId = o.Value.FieldId,
									Name = sections[o.Value.FieldId].Text + " - " + o.Value.Text
								}).OrderBy(s => s.Name).ToList(),

                            Sections = sections
                                .Select(o => new ExtendedCaseSection
                                {
                                    SectionId = o.Value.SectionId,
                                    Name = o.Value.Text
                                }).ToList()
                        };
					}
				}

                return new ReportGeneratorData(settings, sortedOverviews);
            }
        }

        public Dictionary<DateTime, int> GetReportGeneratorAggregation(
             int customerId,
            int userId,
            int languageId,
            List<int> fieldIds,
            List<int> departmentIds,
            List<int> ouIds,
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeIds,
			List<string> extendedCaseFormFieldIds,
			int? extendedCaseFormFieldId,
			DateTime? periodFrom,
            DateTime? periodUntil,
            string text,
            SortField sort,
            int selectCount,
            DateTime? closeFrom,
            DateTime? closeTo)
        {
            using (var uow = this.unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {


                List<Tuple<int, int>> departmentAndOuis = new List<Tuple<int, int>>();

                //Populate the departments
                foreach (var department in departmentIds)
                {
                    departmentAndOuis.Add(new Tuple<int, int>(department, 0));
                }


                //Populate the departments + OUs
                foreach (var ou in ouIds)
                {
                    var ouEntity = _oUService.GetOU(ou);

                    if (ouEntity.Department_Id.HasValue)
                    {
                        departmentAndOuis.Add(new Tuple<int, int>(ouEntity.Department_Id.Value, ou));
                    }
                }



                var caseTypeChainIds = new List<int>();
                foreach (var caseTypeId in caseTypeIds)
                {
                    LoadCaseTypeChildrenIds(caseTypeId, caseTypeChainIds, uow);
                }

                var productAreaChainIds = new List<int>();
                foreach (var productAreaId in productAreaIds)
                {
                    LoadProductAreaChildrenIds(productAreaId, productAreaChainIds, uow);
                }

                //Ensure filters
                departmentAndOuis = EnsureDepartments(departmentAndOuis, userId, customerId);
                workingGroupIds = EnsureWorkingGroups(workingGroupIds, userId, customerId);

                var caseData = _reportRepository.GetCaseAggregation(
                                                customerId,
                                                departmentAndOuis,
                                                workingGroupIds,
                                                productAreaChainIds,
                                                administratorIds,
                                                caseStatusIds,
                                                caseTypeChainIds,
												extendedCaseFormFieldIds,
												extendedCaseFormFieldId,
                                                periodFrom,
                                                periodUntil,
                                                closeFrom,
                                                closeTo);

                return caseData;
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
            CaseRegistrationSource registrationSource,
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

        public List<FullCaseOverview> Sort(List<FullCaseOverview> query, SortField sort)
        {
            if (sort == null)
            {
                return query;
            }
            
            switch (sort.SortBy)
            {
                case SortBy.Ascending:
                    // User
                    if (sort.Name == UserFields.User)
                    {
                        return query.OrderBy(c => c.User.User).ToList();
                    }
                    else if (sort.Name == UserFields.Notifier)
                    {
                        return query.OrderBy(c => c.User.Notifier).ToList();
                    }
                    else if (sort.Name == UserFields.Email)
                    {
                        return query.OrderBy(c => c.User.Email).ToList();
                    }
                    else if (sort.Name == UserFields.Phone)
                    {
                        return query.OrderBy(c => c.User.Phone).ToList();
                    }
                    else if (sort.Name == UserFields.CellPhone)
                    {
                        return query.OrderBy(c => c.User.CellPhone).ToList();
                    }
                    else if (sort.Name == UserFields.Customer)
                    {
                        return query.OrderBy(c => c.User.Customer).ToList();
                    }
                    else if (sort.Name == UserFields.Region)
                    {
                        return query.OrderBy(c => c.User.Region).ToList();
                    }
                    else if (sort.Name == UserFields.Department)
                    {
                        return query.OrderBy(c => c.User.Department).ToList();
                    }
                    else if (sort.Name == UserFields.Unit)
                    {
                        return query.OrderBy(c => c.User.Unit).ToList();
                    }
                    else if (sort.Name == UserFields.Place)
                    {
                        return query.OrderBy(c => c.User.Place).ToList();
                    }
                    else if (sort.Name == UserFields.OrdererCode)
                    {
                        return query.OrderBy(c => c.User.OrdererCode).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_User)
                    {
                        return query.OrderBy(c => c.User.IsAbout_User).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_Name)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Persons_Name).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_Phone)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Persons_Phone).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_UserCode)
                    {
                        return query.OrderBy(c => c.User.IsAbout_UserCode).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_Email)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Persons_Email).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_CellPhone)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Persons_CellPhone).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_CostCentre)
                    {
                        return query.OrderBy(c => c.User.IsAbout_ConstCentre).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Place)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Place).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Department)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Department).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_OU)
                    {
                        return query.OrderBy(c => c.User.IsAbout_OU).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Region)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Region).ToList();
                    }
                    
                    // Computer
                    if (sort.Name == ComputerFields.PcNumber)
                    {
                        return query.OrderBy(c => c.Computer.PcNumber).ToList();
                    }
                    else if (sort.Name == ComputerFields.ComputerType)
                    {
                        return query.OrderBy(c => c.Computer.ComputerType).ToList();
                    }
                    else if (sort.Name == ComputerFields.Place)
                    {
                        return query.OrderBy(c => c.Computer.Place).ToList();
                    }

                    // CaseInfo
                    if (sort.Name == CaseInfoFields.Case)
                    {
                        return query.OrderBy(c => c.CaseInfo.Case).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.RegistrationDate)
                    {
                        return query.OrderBy(c => c.CaseInfo.RegistrationDate).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.ChangeDate)
                    {
                        return query.OrderBy(c => c.CaseInfo.ChangeDate).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.RegisteredBy)
                    {
                        return query.OrderBy(c => c.CaseInfo.RegistratedBy).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.CaseType)
                    {
                        return query.OrderBy(c => c.CaseInfo.CaseType).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.ProductArea)
                    {
                        return query.OrderBy(c => c.CaseInfo.ProductArea).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.System)
                    {
                        return query.OrderBy(c => c.CaseInfo.System).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.UrgentDegree)
                    {
                        return query.OrderBy(c => c.CaseInfo.UrgentDegree).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Impact)
                    {
                        return query.OrderBy(c => c.CaseInfo.Impact).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Category)
                    {
                        return query.OrderBy(c => c.CaseInfo.Category).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Supplier)
                    {
                        return query.OrderBy(c => c.CaseInfo.Supplier).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.InvoiceNumber)
                    {
                        return query.OrderBy(c => c.CaseInfo.InvoiceNumber).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.ReferenceNumber)
                    {
                        return query.OrderBy(c => c.CaseInfo.ReferenceNumber).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Caption)
                    {
                        return query.OrderBy(c => c.CaseInfo.Caption).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Description)
                    {
                        return query.OrderBy(c => c.CaseInfo.Description).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Other)
                    {
                        return query.OrderBy(c => c.CaseInfo.Other).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.PhoneContact)
                    {
                        return query.OrderBy(c => c.CaseInfo.PhoneContact).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Sms)
                    {
                        return query.OrderBy(c => c.CaseInfo.Sms).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.AgreedDate)
                    {
                        return query.OrderBy(c => c.CaseInfo.AgreedDate).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Available)
                    {
                        return query.OrderBy(c => c.CaseInfo.Available).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Cost)
                    {
                        return query.OrderBy(c => c.CaseInfo.Cost).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.AttachedFile)
                    {
                        return query.OrderBy(c => c.CaseInfo.AttachedFile).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.RegistrationSourceCustomer)
                    {
                        return query.OrderBy(c => c.CaseInfo.RegistrationSource).ToList();
                    }

                    // Other
                    if (sort.Name == OtherFields.WorkingGroup)
                    {
                        return query.OrderBy(c => c.Other.WorkingGroup).ToList();
                    }
                    else if (sort.Name == OtherFields.Responsible)
                    {
                        return query.OrderBy(c => c.Other.Responsible).ToList();
                    }
                    else if (sort.Name == OtherFields.Administrator)
                    {
                        return query.OrderBy(c => c.Other.Administrator).ToList();
                    }
                    else if (sort.Name == OtherFields.Priority)
                    {
                        return query.OrderBy(c => c.Other.Priority).ToList();
                    }
                    else if (sort.Name == OtherFields.State)
                    {
                        return query.OrderBy(c => c.Other.State).ToList();
                    }
                    else if (sort.Name == OtherFields.SubState)
                    {
                        return query.OrderBy(c => c.Other.SubState).ToList();
                    }
                    else if (sort.Name == OtherFields.PlannedActionDate)
                    {
                        return query.OrderBy(c => c.Other.PlannedActionDate).ToList();
                    }
                    else if (sort.Name == OtherFields.WatchDate)
                    {
                        return query.OrderBy(c => c.Other.WatchDate).ToList();
                    }
                    else if (sort.Name == OtherFields.Verified)
                    {
                        return query.OrderBy(c => c.Other.Verified).ToList();
                    }
                    else if (sort.Name == OtherFields.VerifiedDescription)
                    {
                        return query.OrderBy(c => c.Other.VerifiedDescription).ToList();
                    }
                    else if (sort.Name == OtherFields.SolutionRate)
                    {
                        return query.OrderBy(c => c.Other.SolutionRate).ToList();
                    }
                    else if (sort.Name == OtherFields.CausingPart)
                    {
                        return query.OrderBy(c => c.Other.CausingPart).ToList();
                    }

                    // Log
                    if (sort.Name == LogFields.FinishingDate)
                    {
                        return query.OrderBy(c => c.Log.ClosingDate).ToList();
                    }

                    if (sort.Name == LogFields.FinishingCause)
                    {
                        return query.OrderBy(c => c.Log.FinishingCause).ToList();
                    }

                    break;

                case SortBy.Descending:
                    // User
                    if (sort.Name == UserFields.User)
                    {
                        return query.OrderByDescending(c => c.User.User).ToList();
                    }
                    else if (sort.Name == UserFields.Notifier)
                    {
                        return query.OrderByDescending(c => c.User.Notifier).ToList();
                    }
                    else if (sort.Name == UserFields.Email)
                    {
                        return query.OrderByDescending(c => c.User.Email).ToList();
                    }
                    else if (sort.Name == UserFields.Phone)
                    {
                        return query.OrderByDescending(c => c.User.Phone).ToList();
                    }
                    else if (sort.Name == UserFields.CellPhone)
                    {
                        return query.OrderByDescending(c => c.User.CellPhone).ToList();
                    }
                    else if (sort.Name == UserFields.Customer)
                    {
                        return query.OrderByDescending(c => c.User.Customer).ToList();
                    }
                    else if (sort.Name == UserFields.Region)
                    {
                        return query.OrderByDescending(c => c.User.Region).ToList();
                    }
                    else if (sort.Name == UserFields.Department)
                    {
                        return query.OrderByDescending(c => c.User.Department).ToList();
                    }
                    else if (sort.Name == UserFields.Unit)
                    {
                        return query.OrderByDescending(c => c.User.Unit).ToList();
                    }
                    else if (sort.Name == UserFields.Place)
                    {
                        return query.OrderByDescending(c => c.User.Place).ToList();
                    }
                    else if (sort.Name == UserFields.OrdererCode)
                    {
                        return query.OrderByDescending(c => c.User.OrdererCode).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_User)
                    {
                        return query.OrderByDescending(c => c.User.IsAbout_User).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_Name)
                    {
                        return query.OrderByDescending(c => c.User.IsAbout_Persons_Name).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_Phone)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Persons_Phone).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_UserCode)
                    {
                        return query.OrderBy(c => c.User.IsAbout_UserCode).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_Email)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Persons_Email).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Persons_CellPhone)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Persons_CellPhone).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_CostCentre)
                    {
                        return query.OrderBy(c => c.User.IsAbout_ConstCentre).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Place)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Place).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Department)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Department).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_OU)
                    {
                        return query.OrderBy(c => c.User.IsAbout_OU).ToList();
                    }
                    else if (sort.Name == UserFields.IsAbout_Region)
                    {
                        return query.OrderBy(c => c.User.IsAbout_Region).ToList();
                    }
                   
                    // Computer
                    if (sort.Name == ComputerFields.PcNumber)
                    {
                        return query.OrderByDescending(c => c.Computer.PcNumber).ToList();
                    }
                    else if (sort.Name == ComputerFields.ComputerType)
                    {
                        return query.OrderByDescending(c => c.Computer.ComputerType).ToList();
                    }
                    else if (sort.Name == ComputerFields.Place)
                    {
                        return query.OrderByDescending(c => c.Computer.Place).ToList();
                    }

                    // CaseInfo
                    if (sort.Name == CaseInfoFields.Case)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Case).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.RegistrationDate)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.RegistrationDate).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.ChangeDate)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.ChangeDate).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.RegisteredBy)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.RegistratedBy).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.CaseType)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.CaseType).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.ProductArea)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.ProductArea).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.System)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.System).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.UrgentDegree)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.UrgentDegree).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Impact)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Impact).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Category)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Category).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Supplier)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Supplier).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.InvoiceNumber)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.InvoiceNumber).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.ReferenceNumber)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.ReferenceNumber).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Caption)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Caption).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Description)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Description).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Other)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Other).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.PhoneContact)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.PhoneContact).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Sms)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Sms).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.AgreedDate)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.AgreedDate).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Available)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Available).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.Cost)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.Cost).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.AttachedFile)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.AttachedFile).ToList();
                    }
                    else if (sort.Name == CaseInfoFields.RegistrationSourceCustomer)
                    {
                        return query.OrderByDescending(c => c.CaseInfo.RegistrationSource).ToList();
                    }

                    // Other
                    if (sort.Name == OtherFields.WorkingGroup)
                    {
                        return query.OrderByDescending(c => c.Other.WorkingGroup).ToList();
                    }
                    else if (sort.Name == OtherFields.Responsible)
                    {
                        return query.OrderByDescending(c => c.Other.Responsible).ToList();
                    }
                    else if (sort.Name == OtherFields.Administrator)
                    {
                        return query.OrderByDescending(c => c.Other.Administrator).ToList();
                    }
                    else if (sort.Name == OtherFields.Priority)
                    {
                        return query.OrderByDescending(c => c.Other.Priority).ToList();
                    }
                    else if (sort.Name == OtherFields.State)
                    {
                        return query.OrderByDescending(c => c.Other.State).ToList();
                    }
                    else if (sort.Name == OtherFields.SubState)
                    {
                        return query.OrderByDescending(c => c.Other.SubState).ToList();
                    }
                    else if (sort.Name == OtherFields.PlannedActionDate)
                    {
                        return query.OrderByDescending(c => c.Other.PlannedActionDate).ToList();
                    }
                    else if (sort.Name == OtherFields.WatchDate)
                    {
                        return query.OrderByDescending(c => c.Other.WatchDate).ToList();
                    }
                    else if (sort.Name == OtherFields.Verified)
                    {
                        return query.OrderByDescending(c => c.Other.Verified).ToList();
                    }
                    else if (sort.Name == OtherFields.VerifiedDescription)
                    {
                        return query.OrderByDescending(c => c.Other.VerifiedDescription).ToList();
                    }
                    else if (sort.Name == OtherFields.SolutionRate)
                    {
                        return query.OrderByDescending(c => c.Other.SolutionRate).ToList();
                    }
                    else if (sort.Name == OtherFields.CausingPart)
                    {
                        return query.OrderByDescending(c => c.Other.CausingPart).ToList();
                    }

                    // Log
                    if (sort.Name == LogFields.FinishingDate)
                    {
                        return query.OrderByDescending(c => c.Log.ClosingDate).ToList();
                    }

                    if (sort.Name == LogFields.FinishingCause)
                    {
                        return query.OrderByDescending(c => c.Log.FinishingCause).ToList();
                    }

                    break;
            }            

            return query;
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
                LoadCaseTypeChildrenIds(child.Id, ids, uow);
            }
        }

        private List<Tuple<int, int>> EnsureDepartments(List<Tuple<int, int>> departmentsAndOUs, int userId, int customerId)
        {
            if (departmentsAndOUs.Any())
            {
                return departmentsAndOUs;
            }
            else
            {

                List<Tuple<int, int>> ret = new List<Tuple<int, int>>();

                var allowedDepartmentIds = _departmentService.GetDepartmentsByUserPermissions(userId, customerId, false).Select(x => x.Id).ToList();

                foreach (var department in allowedDepartmentIds)
                {
                    ret.Add(new Tuple<int, int>(department, 0));
                }

                return ret;
            }
        }

        private List<int> EnsureWorkingGroups(List<int> workingGroups, int userId, int customerId)
        {
            var user = _userService.GetUser(userId);            
            var ret = new List<int>();

            if (workingGroups.Any())                                
                ret = workingGroups;
            else
            {
                var allowedWorkingGroups = new List<int>();
                /*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */
                
                if (user.UserGroup_Id > UserGroups.Administrator)
                    allowedWorkingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).Select(w => w.Id).ToList();
                else
                {
                    allowedWorkingGroups = _workingGroupService.GetWorkingGroups(customerId, userId, false).Select(x => x.Id).ToList();
                    /* If allowed wg is empty, means user has no access to see any case. so we make a false condition */                    
                }
                ret = allowedWorkingGroups;
            }

            if (user.UserGroup_Id > UserGroups.Administrator || (user.ShowNotAssignedWorkingGroups != 0))
                ret.Add(0); //Not assigned wg

            return ret;
        }

		#endregion
	}
}