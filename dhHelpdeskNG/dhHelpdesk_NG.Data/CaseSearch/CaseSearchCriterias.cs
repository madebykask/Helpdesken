using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Dal.Repositories
{
    public class CaseSearchCriterias : ICaseSearchCriterias
    {
        public string ApplicationType { get; set; }
        public ISearch Search { get; set; }
        public CaseSearchFilter SearchFilter { get; set; }
        public Setting CustomerSetting { get; set; }
        public CustomerUser CustomerUserSettings { get; set; }
        public bool IsFieldResponsibleVisible { get; set; }
        public int UserId { get; set; }
        public string UserUniqueId { get; set; }
        public int ShowNotAssignedWorkingGroups { get; set; }
        public int UserGroupId { get; set; }
        public GlobalSetting GlobalSetting { get; set; }
        public int? RelatedCasesCaseId { get; set; }
        public Dictionary<string, CaseSettings> CaseSettings { get; set; }
        public IList<Department> UserDepartments { get; set; }
        public string RelatedCasesUserId { get; set; }
        public int[] CaseIds { get; set; }
        public IList<int> CaseTypes { get; set; }
        public bool FetchInfoAboutParentChild { get; set; }

		public bool HasAccessToInternalLogNotes { get; set; }
        public bool IncludeExtendedCaseValues { get; set; }

	}
}