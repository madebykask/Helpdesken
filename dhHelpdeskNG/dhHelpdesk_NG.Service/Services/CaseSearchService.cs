namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Holiday.Output;
    using DH.Helpdesk.Dal.Repositories;
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
            IEnumerable<HolidayOverview> holidays);
    }

    public class CaseSearchService : ICaseSearchService
    {
        private readonly ICaseSearchRepository _caseSearchRepository;
        private readonly IProductAreaService _productAreaService;
        private readonly IGlobalSettingService _globalSettingService;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;

        public CaseSearchService(ICaseSearchRepository caseSearchRepository, IProductAreaService productAreaService, IGlobalSettingService globalSettingService, ISettingService settingService, IUserService userService)
        {
            this._caseSearchRepository = caseSearchRepository;
            this._globalSettingService = globalSettingService;
            this._settingService = settingService;
            this._productAreaService = productAreaService;
            this._userService = userService;
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
                                IEnumerable<HolidayOverview> holidays)
        {
            int productAreaId;
            var csf = new CaseSearchFilter();
            csf = csf.Copy(f);

            // ärenden som tillhör barn till föräldrer ska visas om vi filtrerar på föräldern
            if (int.TryParse(csf.ProductArea, out productAreaId))
                csf.ProductArea = this._productAreaService.GetProductAreaWithChildren(productAreaId, ", ", "Id");

            return this._caseSearchRepository.Search(
                                                csf, 
                                                csl, 
                                                userId, 
                                                userUserId, 
                                                showNotAssignedWorkingGroups, 
                                                userGroupId, 
                                                restrictedCasePermission, 
                                                this._globalSettingService.GetGlobalSettings().FirstOrDefault(), 
                                                this._settingService.GetCustomerSetting(f.CustomerId), 
                                                s,
                                                workingDayStart,
                                                workingDayEnd,
                                                holidays);
        }

    }
}