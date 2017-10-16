namespace DH.Helpdesk.Web.Enums
{
    public class CaseFilterFields
    {
        public const string InitiatorNameAttribute = "CaseInitiatorFilter";
		public const string InitiatorSearchScopeAttribute = "InitiatorSearchScope";
		public const string FreeTextSearchNameAttribute = "txtFreeTextSearch";        

        public const string RegionNameAttribute = "lstFilterRegion";        
        public const string DepartmentNameAttribute = "lstfilterDepartment";        
        public const string RegisteredByNameAttribute = "lstfilterUser";                
        public const string CategoryNameAttribute = "lstfilterCategory";
        public const string WorkingGroupNameAttribute = "lstfilterWorkingGroup";
        public const string ResponsibleNameAttribute = "lstfilterResponsible";
        public const string PerformerNameAttribute = "lstfilterPerformer";
        public const string PriorityNameAttribute = "lstfilterPriority";
        public const string StatusNameAttribute = "lstfilterStatus";
        public const string StateSecondaryNameAttribute = "lstfilterStateSecondary";
        public const string CaseRemainingTimeAttribute = "lstfilterCaseRemainingTime";

        public const string FilterCaseProgressNameAttribute = "lstfilterCaseProgress";
        public const string CaseFilterFavoriteNameAttribute = "lstMyFavorites";

        public const string CaseRegistrationDateStartFilterNameAttribute = "CaseRegistrationDateStartFilter";
        public const string CaseRegistrationDateEndFilterFilterNameAttribute = "CaseRegistrationDateEndFilter";
        public const string CaseWatchDateStartFilterNameAttribute = "CaseWatchDateStartFilter";
        public const string CaseWatchDateEndFilterNameAttribute = "CaseWatchDateEndFilter";
        public const string CaseClosingDateStartFilterNameAttribute = "CaseClosingDateStartFilter";
        public const string CaseClosingDateEndFilterNameAttribute = "CaseClosingDateEndFilter";
        
        

        public const string CaseTypeIdNameAttribute = "hidFilterCaseTypeId";
        public const string ClosingReasonNameAttribute = "hidFilterClosingReasonId";
        public const string ProductAreaIdNameAttribute = "hidFilterProductAreaId";

		public const string OrderColumnNum = "order";
		public const string OrderColumnDir = "dir";
		public const string PageStart = "start";
		public const string PageSize = "length";

        public const string IsConnectToParent = "IsConnectToParent";
        public const string IsExtendedSearch = "extendedSearchEnabled";
        public const string CurrentCaseId = "currentCaseId";


    }
}