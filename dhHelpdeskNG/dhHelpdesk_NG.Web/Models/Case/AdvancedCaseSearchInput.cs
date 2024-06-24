using System;
using DH.Helpdesk.BusinessData.Enums.Case;

namespace DH.Helpdesk.Web.Models.Case
{
    public class AdvancedCaseSearchInput
    {
        public int CustomerId { get; set; }
        public bool IsExtendedSearch { get; set; }
        public string Customers { get; set; }
        public string CaseProgress { get; set; }
        public string UserPerformer { get; set; }
        public string Initiator { get; set; }
        public CaseInitiatorSearchScope InitiatorSearchScope { get; set; } // todo use enum instead

        public DateTime? CaseRegistrationDateStartFilter { get; set; }
        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }
        public DateTime? CaseClosingDateEndFilter { get; set; }
        
        public string FreeTextSearch { get; set; }
        public string CaptionSearch { get; set; }
        public string CaseNumber { get; set; }
        public bool SearchThruFiles { get; set; }

        public bool IncludeExtendedCaseValues { get; set; }

        //customer seach fields
        public string WorkingGroup { get; set; }
        public string Department { get; set; }
        public string Priority { get; set; }
        public string StateSecondary { get; set; }
        public int? CaseType { get; set; }
        public string ProductArea { get; set; }
        public string CaseClosingReasonFilter { get; set; }
        
        public int MaxRows { get; set; }
        public string SortBy { get; set; }
        public int SortDir { get; set; }
        public int PageIndex { get; set; }
        public int RecordsPerPage { get; set; }

        /*
        public int UserId { get; set; }
        public string Region { get; set; }
        public string OrganizationUnit { get; set; }
        public string User { get; set; }
        public string UserResponsible { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string CaseFilterFavorite { get; set; }
        public string CaseRemainingTime { get; set; }
     
        public string ParantPath_ProductArea = DropDownButtonDefaultValue;
        public string ParantPath_Category = DropDownButtonDefaultValue;
        public string ParantPath_CaseType = DropDownButtonDefaultValue;
        public string ParentPathClosingReason = DropDownButtonDefaultValue;
        public string ReportedBy { get; set; }
        public string RegUserId { get; set; }
        public string MaxRows { get; set; }
        public DateTime? CaseWatchDateStartFilter { get; set; }
        public DateTime? CaseWatchDateEndFilter { get; set; }


        

        

        public string CaseListType { get; set; }

        public CasesCustomFilter CustomFilter { get; set; }

        public bool SearchInMyCasesOnly { get; set; }

        public int? CaseRemainingTimeFilter { get; set; }
        public int? CaseRemainingTimeUntilFilter { get; set; }

        public int? CaseRemainingTimeMaxFilter { get; set; }
        public bool CaseRemainingTimeHoursFilter { get; set; }

        public int MaxTextCharacters { get; set; }

        public PageInfo PageInfo { get; set; }

        public bool IsConnectToParent { get; set; }
        public int? CurrentCaseId { get; set; }

        public CaseOverviewCriteriaModel CaseOverviewCriteria { get; set; }

        public bool FetchInfoAboutParentChild { get; set; }
        public string SortBy { get; set; }
        public bool Ascending { get; set; }*/
    }
}