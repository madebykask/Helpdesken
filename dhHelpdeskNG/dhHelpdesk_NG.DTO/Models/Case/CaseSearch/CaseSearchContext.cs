using System;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseSearch
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.BusinessData.Models.WorktimeCalculator;
    using DH.Helpdesk.Domain;

    public class CaseSearchContext
    {
        public CaseSearchFilter f;

        public IList<CaseSettings> userCaseSettings;

        public bool isFieldResponsibleVisible;

        public int userId;

        public string userUserId;

        public int WorkingHours;

        public int showNotAssignedWorkingGroups;

        public int userGroupId;

        public bool restrictedCasePermission;

        public GlobalSetting globalSettings;

        public Setting customerSetting;

        public ISearch s;

        public IWorkTimeCalculatorFactory workTimeCalcFactory;

        public string applicationType;

        public bool calculateRemainingTime;

        public IProductAreaNameResolver productAreaNamesResolver;

        public int? relatedCasesCaseId = null;

        public string relatedCasesUserId = null;

        public int[] caseIds = null;

        public DateTime now;

        public bool useFullTextSearch;

		public bool hasAccessToInternalLogNotes;

        public bool includeExtendedCaseValues;
    }
}
