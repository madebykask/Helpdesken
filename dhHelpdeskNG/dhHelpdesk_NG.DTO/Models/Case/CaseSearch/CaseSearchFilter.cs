namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;

    using DH.Helpdesk.BusinessData.Enums.Case;

    public class CaseSearchFilter
    {
        #region Form inputs names
        public const string InitiatorNameAttribute = "CaseInitiatorFilter";
        public const string RegionNameAttribute = "lstFilterRegion";
        
        public const string DepartmentNameAttribute = "lstfilterDepartment";
        public const string RegisteredByNameAttribute = "lstfilterUser";
        public const string CaseTypeIdNameAttribute = "hidFilterCaseTypeId";
        public const string ProductAreaIdNameAttribute = "hidFilterProductAreaId";
        public const string CategoryNameAttribute = "lstfilterCategory";
        public const string WorkingGroupNameAttribute = "lstfilterWorkingGroup";
        public const string ResponsibleNameAttribute = "lstfilterResponsible";
        public const string PerformerNameAttribute = "lstfilterPerformer";
        public const string PriorityNameAttribute = "lstfilterPriority";
        public const string StatusNameAttribute = "lstfilterStatus";
        public const string StateSecondaryNameAttribute = "lstfilterStateSecondary";

        public const string CaseRegistrationDateStartFilterNameAttribute = "CaseRegistrationDateStartFilter";
        public const string CaseRegistrationDateEndFilterFilterNameAttribute = "CaseRegistrationDateEndFilter";
        public const string CaseWatchDateStartFilterNameAttribute = "CaseWatchDateStartFilter";
        public const string CaseWatchDateEndFilterNameAttribute = "CaseWatchDateEndFilter";
        public const string CaseClosingDateStartFilterNameAttribute = "CaseClosingDateStartFilter";
        public const string CaseClosingDateEndFilterNameAttribute = "CaseClosingDateEndFilter";

        public const string ClosingReasonNameAttribute = "hidFilterClosingReasonId";
        public const string FreeTextSearchNameAttribute = "txtFreeTextSearch";
        public const string FilterCaseProgressNameAttribute = "lstfilterCaseProgress";
        #endregion

        #region Case state filter string constants, aka Case progress
        public const string UnreadCases = "4";
        public const string HoldCases = "3";
        public const string InProgressCases = "2";
        #endregion

        public const string DropDownButtonDefaultValue = "--";

        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string ProductArea { get; set; }
        public int CaseType { get; set; }
        public string Region { get; set; }
        public string Department { get; set; }
        public string User { get; set; }

        /// <summary>
        /// List of case adminstrators id joined with ','
        /// </summary>
        public string UserPerformer { get; set; }
        public string UserResponsible { get; set; }

        /// <summary>
        /// String that will be searched in "Initiator" case field
        /// </summary>
        public string Initiator { get; set; }

        public string Category { get; set; }
        public string WorkingGroup { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string StateSecondary { get; set; }
        public string CaseProgress { get; set; }
        public string FreeTextSearch { get; set; }

        public string ParantPath_ProductArea = DropDownButtonDefaultValue;

        public string ParantPath_CaseType = DropDownButtonDefaultValue;

        public string ParentPathClosingReason = DropDownButtonDefaultValue;

        public string ReportedBy { get; set; }
        public string RegUserId { get; set; }
        public string Customer { get; set; }
        public string CaseNumber { get; set; }
        public string MaxRows { get; set; }

        public DateTime? CaseRegistrationDateStartFilter { get; set; }

        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseWatchDateStartFilter { get; set; }

        public DateTime? CaseWatchDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }

        public DateTime? CaseClosingDateEndFilter { get; set; }

        public string CaseClosingReasonFilter { get; set; }

        public string CaseListType { get; set; }

        public CasesCustomFilter CustomFilter { get; set; }

        public bool SearchInMyCasesOnly { get; set; }

        public int? CaseRemainingTimeFilter { get; set; }

        public int? CaseRemainingTimeUntilFilter { get; set; }

        public int? CaseRemainingTimeMaxFilter { get; set; }

        public bool CaseRemainingTimeHoursFilter { get; set; }

        public CaseSearchFilter Copy(CaseSearchFilter o)
        {
            var r = new CaseSearchFilter();
            
            r.CaseProgress = o.CaseProgress;
            r.CaseType = o.CaseType;
            r.Category = o.Category;
            r.CustomerId = o.CustomerId;
            r.Department = o.Department;
            r.FreeTextSearch = o.FreeTextSearch;
            r.ParantPath_CaseType = o.ParantPath_CaseType;
            r.ParantPath_ProductArea = o.ParantPath_ProductArea; 
            r.Priority = o.Priority;
            r.ProductArea = o.ProductArea;
            r.Region = o.Region;
            r.StateSecondary = o.StateSecondary;
            r.Status = o.Status;
            r.User = o.User;
            r.UserId = o.UserId;
            r.UserPerformer = o.UserPerformer;
            r.UserResponsible = o.UserResponsible;
            r.WorkingGroup = o.WorkingGroup;
            r.ReportedBy = o.ReportedBy;
            r.CaseListType = o.CaseListType;  
            r.CaseRegistrationDateStartFilter = o.CaseRegistrationDateStartFilter;    
            r.CaseRegistrationDateEndFilter = o.CaseRegistrationDateEndFilter;    
            r.CaseWatchDateStartFilter = o.CaseWatchDateStartFilter;    
            r.CaseWatchDateEndFilter = o.CaseWatchDateEndFilter;    
            r.CaseClosingDateStartFilter = o.CaseClosingDateStartFilter;
            r.CaseClosingDateEndFilter = o.CaseClosingDateEndFilter;
            r.CaseClosingReasonFilter = o.CaseClosingReasonFilter;
            r.ParentPathClosingReason = o.ParentPathClosingReason;
            r.CustomFilter = o.CustomFilter;
            r.CaseRemainingTimeFilter = o.CaseRemainingTimeFilter;
            r.CaseRemainingTimeUntilFilter = o.CaseRemainingTimeUntilFilter;
            r.CaseRemainingTimeMaxFilter = o.CaseRemainingTimeMaxFilter;
            r.CaseRemainingTimeHoursFilter = o.CaseRemainingTimeHoursFilter;
            r.Initiator = o.Initiator;
            r.SearchInMyCasesOnly = o.SearchInMyCasesOnly;
            r.Customer = o.Customer;
            r.CaseNumber = o.CaseNumber;
            r.MaxRows = o.MaxRows;

            return r;
        }
    }
    
}

