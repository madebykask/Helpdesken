using DH.Helpdesk.BusinessData.Models.Case.CaseIntLog;
using DH.Helpdesk.BusinessData.Models.Shared.Output;

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Case.CaseSearch;
    using DH.Helpdesk.BusinessData.Models.Grid;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Utils;

    public interface ICaseSearchService
    {
        SearchResult<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> csl,
            CaseFieldSetting[] customerCaseFieldsSettings,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            int restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            TimeZoneInfo userTimeZone,
            string applicationType);

        SearchResult<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> csl,
            CaseFieldSetting[] customerCaseFieldsSettings,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            int restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            TimeZoneInfo userTimeZone,
            string applicationType,
            bool calculateRemainingTime,
            out CaseRemainingTimeData remainingTime,            
            out CaseAggregateData aggregateData,
            int? relatedCasesCaseId = null,
            string relatedCasesUserId = null,
            int[] caseIds = null);

        List<CaseEmailSendOverview> GetUserEmailsForCaseSend(int customerId, string query, bool searchInWorkingGrs, bool searchInInitiators, bool searchInAdmins, bool searchInEmailGrs, bool isInternalLog = false);
    }

    public class CaseSearchService : ICaseSearchService
    {
        private readonly ICaseSearchRepository caseSearchRepository;
        private readonly IUserEmailRepository _userEmailRepository;
        private readonly IProductAreaService productAreaService;
        private readonly IGlobalSettingService globalSettingService;
        private readonly ISettingService settingService;

        private readonly IHolidayService holidayService;

        public CaseSearchService(
            ICaseSearchRepository caseSearchRepository,
            IUserEmailRepository userEmailRepository, 
            IProductAreaService productAreaService, 
            IGlobalSettingService globalSettingService, 
            ISettingService settingService,
            IHolidayService holidayService)
        {
            this.caseSearchRepository = caseSearchRepository;
            this._userEmailRepository = userEmailRepository;
            this.globalSettingService = globalSettingService;
            this.settingService = settingService;
            this.holidayService = holidayService;
            this.productAreaService = productAreaService;
        }

        public SearchResult<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> csl,
            CaseFieldSetting[] customerCaseFieldsSettings,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            int restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            TimeZoneInfo userTimeZone,
            string applicationType)
        {
            CaseRemainingTimeData remainingTime;
            CaseAggregateData aggregateData;

            return this.Search(
                        f,
                        csl,
                        customerCaseFieldsSettings,
                        userId,
                        userUserId,
                        showNotAssignedWorkingGroups,
                        userGroupId,
                        restrictedCasePermission,
                        s,
                        workingDayStart,
                        workingDayEnd,
                        userTimeZone,
                        applicationType,
                        false,
                        out remainingTime,
                        out aggregateData);
        }

        public SearchResult<CaseSearchResult> Search(
                                CaseSearchFilter f, 
                                IList<CaseSettings> csl, 
                                CaseFieldSetting[] customerCaseFieldsSettings,
                                int userId, 
                                string userUserId, 
                                int showNotAssignedWorkingGroups, 
                                int userGroupId, 
                                int restrictedCasePermission, 
                                ISearch s,
                                int workingDayStart,
                                int workingDayEnd,
                                TimeZoneInfo userTimeZone,
                                string applicationType,
                                bool calculateRemainingTime,
                                out CaseRemainingTimeData remainingTime,
                                out CaseAggregateData aggregateData,
                                int? relatedCasesCaseId = null,
                                string relatedCasesUserId = null,
                                int[] caseIds = null)
        {
            
            var csf = DoFilterValidation(f);            
            
            var workTimeFactory = new WorkTimeCalculatorFactory(this.holidayService, workingDayStart, workingDayEnd, userTimeZone);
            var resonisbleFieldSettings = customerCaseFieldsSettings.Where(it => it.Name == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString()).FirstOrDefault();
            var isFieldResponsibleVisible =
                resonisbleFieldSettings != null && resonisbleFieldSettings.ShowOnStartPage == 1;

            var context = new CaseSearchContext()
                              {
                                    f = csf,
                                    userCaseSettings = csl,
                                    isFieldResponsibleVisible = isFieldResponsibleVisible,
                                    userId = userId,
                                    userUserId = userUserId,
                                    showNotAssignedWorkingGroups = showNotAssignedWorkingGroups,
                                    userGroupId = userGroupId,
                                    restrictedCasePermission = restrictedCasePermission,
                                    gs = this.globalSettingService.GetGlobalSettings().FirstOrDefault(),
                                    customerSetting = this.settingService.GetCustomerSetting(f.CustomerId),
                                    s = s,
                                    workTimeCalcFactory = workTimeFactory,
                                    applicationType = applicationType,
                                    calculateRemainingTime = calculateRemainingTime,
                                    productAreaNamesResolver = this.productAreaService,
                                    relatedCasesCaseId = relatedCasesCaseId,
                                    relatedCasesUserId = relatedCasesUserId,
                                    caseIds = caseIds
                              };

			var workingHours = workingDayEnd - workingDayStart;
			var result = this.caseSearchRepository.Search(context, workingHours, out remainingTime, out aggregateData);

            return result;
        }

        public List<CaseEmailSendOverview> GetUserEmailsForCaseSend(int customerId, string query, bool searchInWorkingGrs, bool searchInInitiators, bool searchInAdmins, bool searchInEmailGrs, bool isInternalLog = false)
        {
            return _userEmailRepository.GetUserEmailsListForCaseSend(customerId, query, searchInWorkingGrs, searchInInitiators, searchInAdmins, searchInEmailGrs, isInternalLog);
        }

        private CaseSearchFilter DoFilterValidation(CaseSearchFilter filter)
        {
            var filterValidate = filter.Copy(filter);

            //Applied in FreeTextSearchSafeForSQLInject
            //if (!string.IsNullOrEmpty(filterValidate.FreeTextSearch))
            //    filterValidate.FreeTextSearch = filterValidate.FreeTextSearch.Replace("'", "''");

            //if (!string.IsNullOrEmpty(filterValidate.CaptionSearch))
            //    filterValidate.CaptionSearch = filterValidate.CaptionSearch.Replace("'", "''");

            // ärenden som tillhör barn till föräldrer ska visas om vi filtrerar på föräldern
            int productAreaId;
            if (int.TryParse(filter.ProductArea, out productAreaId))
            {
                filterValidate.ProductArea = this.productAreaService.GetProductAreaWithChildren(productAreaId, ", ", "Id");
            }
            return filterValidate;
        }
    }
}