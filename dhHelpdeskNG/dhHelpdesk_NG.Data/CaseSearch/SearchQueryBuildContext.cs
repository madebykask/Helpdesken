using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case.CaseSearch;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories
{
    public class SearchQueryBuildContext
    {
        public ICaseSearchCriterias Criterias { get; private set; }

        #region ctor()

        public SearchQueryBuildContext(CaseSearchContext context, 
                                       CustomerUser customerUserSettings, 
                                       Dictionary<string, CaseSettings> userCaseSettings, 
                                       IList<Department> userDepartments, 
                                       List<int> caseTypes)
        {

			Criterias = new CaseSearchCriterias
			{
				ApplicationType = context.applicationType,
				SearchFilter = context.f,
				Search = context.s,
				CustomerSetting = context.customerSetting,
				IsFieldResponsibleVisible = context.isFieldResponsibleVisible,
				UserId = context.userId,
				UserUniqueId = context.userUserId,
				ShowNotAssignedWorkingGroups = context.showNotAssignedWorkingGroups,
				UserGroupId = context.userGroupId,
				GlobalSetting = context.globalSettings,
				RelatedCasesCaseId = context.relatedCasesCaseId,
				CaseSettings = userCaseSettings,
				RelatedCasesUserId = context.relatedCasesUserId,
				CaseIds = context.caseIds,

				CustomerUserSettings = customerUserSettings,
				UserDepartments = userDepartments,
				CaseTypes = caseTypes,
				FetchInfoAboutParentChild = context.f.FetchInfoAboutParentChild,
				HasAccessToInternalLogNotes = context.hasAccessToInternalLogNotes,
				IncludeExtendedCaseValues = context.includeExtendedCaseValues
            };

			
            UseFullTextSearch = context.useFullTextSearch;
            
        }

        #endregion

        public bool UseFreeTextCaseSearchCTE { get; set; }
        public bool UseFullTextSearch { get; }
    }
}