using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories
{
    public interface ICaseSearchCriterias
    {
        string ApplicationType { get; }
        CaseSearchFilter SearchFilter { get; }
        ISearch Search { get; }
        Setting CustomerSetting { get; }
        CustomerUser CustomerUserSettings { get; }
        bool IsFieldResponsibleVisible { get; }
        int UserId { get; }
        string UserUniqueId { get; }
        int ShowNotAssignedWorkingGroups { get; }
        int UserGroupId { get; }
        GlobalSetting GlobalSetting { get; }
        int? RelatedCasesCaseId { get; }
        Dictionary<string, CaseSettings> CaseSettings { get; }
        IList<Department> UserDepartments { get; }
        string RelatedCasesUserId { get; }
        int[] CaseIds { get; }
        IList<int> CaseTypes { get; }
        bool FetchInfoAboutParentChild { get; set; }
		bool HasAccessToInternalLogNotes { get; set; }
        bool IncludeExtendedCaseValues { get; set; }

    }
}