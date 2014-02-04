using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.DTOs.Case;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseSearchService
    {
        IList<CaseSearchResult> Search(CaseSearchFilter f, IList<CaseSettings> csl, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, int restrictedCasePermission, ISearch s);
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
            _caseSearchRepository = caseSearchRepository;
            _globalSettingService = globalSettingService;
            _settingService = settingService;
            _productAreaService = productAreaService;
            _userService = userService;
        }

        public IList<CaseSearchResult> Search(CaseSearchFilter f, IList<CaseSettings> csl, int userId, string userUserId, int showNotAssignedWorkingGroups, int userGroupId, int restrictedCasePermission, ISearch s)
        {
            int productAreaId;
            var csf = new CaseSearchFilter();
            csf = csf.Copy(f);

            // ärenden som tillhör barn till föräldrer ska visas om vi filtrerar på föräldern
            if (int.TryParse(csf.ProductArea, out productAreaId))
                csf.ProductArea = _productAreaService.GetProductAreaWithChildren(productAreaId, ", ", "Id");

            return _caseSearchRepository.Search(csf, csl, userId, userUserId, showNotAssignedWorkingGroups, userGroupId, restrictedCasePermission, _globalSettingService.GetGlobalSettings().FirstOrDefault(), _settingService.GetCustomerSetting(f.CustomerId), s);
        }

    }
}