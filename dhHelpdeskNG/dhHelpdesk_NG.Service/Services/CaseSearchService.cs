namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Utils;
    using DH.Helpdesk.Domain;

    public interface ICaseSearchService
    {
        IList<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> csl,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            int restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            WorkTimeCalculator workTimeCalculator,
            string applicationType);

        IList<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> csl,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            int restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            WorkTimeCalculator workTimeCalculator,
            string applicationType,
            bool calculateRemainingTime,
            out CaseRemainingTimeData remainingTime,
            int? relatedCasesCaseId = null,
            string relatedCasesUserId = null);
    }

    public class CaseSearchService : ICaseSearchService
    {
        private readonly ICaseSearchRepository caseSearchRepository;
        private readonly IProductAreaService productAreaService;
        private readonly IGlobalSettingService globalSettingService;
        private readonly ISettingService settingService;

        public CaseSearchService(
            ICaseSearchRepository caseSearchRepository, 
            IProductAreaService productAreaService, 
            IGlobalSettingService globalSettingService, 
            ISettingService settingService)
        {
            this.caseSearchRepository = caseSearchRepository;
            this.globalSettingService = globalSettingService;
            this.settingService = settingService;
            this.productAreaService = productAreaService;
        }

        public IList<CaseSearchResult> Search(
            CaseSearchFilter f,
            IList<CaseSettings> csl,
            int userId,
            string userUserId,
            int showNotAssignedWorkingGroups,
            int userGroupId,
            int restrictedCasePermission,
            ISearch s,
            int workingDayStart,
            int workingDayEnd,
            WorkTimeCalculator workTimeCalculator,
            string applicationType)
        {
            CaseRemainingTimeData remainingTime;
            return this.Search(
                        f,
                        csl,
                        userId,
                        userUserId,
                        showNotAssignedWorkingGroups,
                        userGroupId,
                        restrictedCasePermission,
                        s,
                        workingDayStart,
                        workingDayEnd,
                        workTimeCalculator,
                        applicationType,
                        false,
                        out remainingTime);
        }

        public IList<CaseSearchResult> Search(
                                CaseSearchFilter f, 
                                IList<CaseSettings> csl, 
                                int userId, 
                                string userUserId, 
                                int showNotAssignedWorkingGroups, 
                                int userGroupId, 
                                int restrictedCasePermission, 
                                ISearch s,
                                int workingDayStart,
                                int workingDayEnd,
                                WorkTimeCalculator workTimeCalculator,
                                string applicationType,
                                bool calculateRemainingTime,
                                out CaseRemainingTimeData remainingTime,
                                int? relatedCasesCaseId = null,
                                string relatedCasesUserId = null)
        {
            int productAreaId;
            var csf = new CaseSearchFilter();
            csf = csf.Copy(f);

            // ärenden som tillhör barn till föräldrer ska visas om vi filtrerar på föräldern
            if (int.TryParse(csf.ProductArea, out productAreaId))
            {
                csf.ProductArea = this.productAreaService.GetProductAreaWithChildren(productAreaId, ", ", "Id");
            }
    
            var result = this.caseSearchRepository.Search(
                                                csf, 
                                                csl, 
                                                userId, 
                                                userUserId, 
                                                showNotAssignedWorkingGroups, 
                                                userGroupId, 
                                                restrictedCasePermission, 
                                                this.globalSettingService.GetGlobalSettings().FirstOrDefault(), 
                                                this.settingService.GetCustomerSetting(f.CustomerId), 
                                                s,
                                                workTimeCalculator,
                                                applicationType,
                                                calculateRemainingTime,
                                                this.productAreaService,
                                                out remainingTime,
                                                relatedCasesCaseId,
                                                relatedCasesUserId);

            var workingHours = workingDayEnd - workingDayStart;
            if (f.CaseRemainingTimeFilter.HasValue && calculateRemainingTime)
            {
                IEnumerable<CaseRemainingTime> filteredCaseRemainigTimes;
                if (f.CaseRemainingTimeFilter < 0)
                {
                    filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime < 0);
                }
                else if (f.CaseRemainingTimeHoursFilter)
                {
                    if (f.CaseRemainingTimeUntilFilter.HasValue)
                    {
                        filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime >= f.CaseRemainingTimeFilter.Value && t.RemainingTime < f.CaseRemainingTimeUntilFilter);
                    }
                    else
                    {
                        filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime == f.CaseRemainingTimeFilter.Value - 1);
                    }
                }
                else if (f.CaseRemainingTimeFilter == int.MaxValue && f.CaseRemainingTimeMaxFilter.HasValue)
                {
                    filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime.IsHoursGreaterEqualDays(f.CaseRemainingTimeMaxFilter.Value, workingHours));
                }
                else 
                {
                    filteredCaseRemainigTimes = remainingTime.CaseRemainingTimes.Where(t => t.RemainingTime.IsHoursEqualDays(f.CaseRemainingTimeFilter.Value - 1, workingHours));
                }

                result = result.Where(c => filteredCaseRemainigTimes.Select(t => t.CaseId).Contains(c.Id)).ToList();
            }

            return result;
        }
    }
}