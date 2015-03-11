namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
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
            string applicationId = null);
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
                                string applicationId = null)
        {
            int productAreaId;
            var csf = new CaseSearchFilter();
            csf = csf.Copy(f);

            // ärenden som tillhör barn till föräldrer ska visas om vi filtrerar på föräldern
            if (int.TryParse(csf.ProductArea, out productAreaId))
            {
                csf.ProductArea = this.productAreaService.GetProductAreaWithChildren(productAreaId, ", ", "Id");
            }

            return this.caseSearchRepository.Search(
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
                                                applicationId);
        }
    }
}