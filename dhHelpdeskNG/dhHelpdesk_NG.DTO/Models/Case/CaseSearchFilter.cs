
namespace DH.Helpdesk.BusinessData.Models.Case
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Case;

    public class CaseSearchFilter 
    {
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string ProductArea { get; set; }
        public int CaseType { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Department { get; set; }
        public string User { get; set; }
        public string UserPerformer { get; set; }
        public string UserResponsible { get; set; }
        public string Category { get; set; }
        public string WorkingGroup { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public string StateSecondary { get; set; }
        public string CaseProgress { get; set; }
        public string FreeTextSearch { get; set; }
        public string ParantPath_ProductArea { get; set; }
        public string ParantPath_CaseType { get; set; }
        public string ReportedBy { get; set; }
        public string RegUserId { get; set; }        

        public DateTime? CaseRegistrationDateStartFilter { get; set; }

        public DateTime? CaseRegistrationDateEndFilter { get; set; }

        public DateTime? CaseWatchDateStartFilter { get; set; }

        public DateTime? CaseWatchDateEndFilter { get; set; }

        public DateTime? CaseClosingDateStartFilter { get; set; }

        public DateTime? CaseClosingDateEndFilter { get; set; }

        public string CaseClosingReasonFilter { get; set; }

        public string ParentPathClosingReason { get; set; }

        public string LMCaseList { get; set; }

        public CasesCustomFilter CustomFilter { get; set; }

        public CaseSearchFilter Copy(CaseSearchFilter o)
        {
            var r = new CaseSearchFilter();
            
            r.CaseProgress = o.CaseProgress;
            r.CaseType = o.CaseType;
            r.Category = o.Category;
            r.Country = o.Country;
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
            r.LMCaseList = o.LMCaseList;  
            r.CaseRegistrationDateStartFilter = o.CaseRegistrationDateStartFilter;    
            r.CaseRegistrationDateEndFilter = o.CaseRegistrationDateEndFilter;    
            r.CaseWatchDateStartFilter = o.CaseWatchDateStartFilter;    
            r.CaseWatchDateEndFilter = o.CaseWatchDateEndFilter;    
            r.CaseClosingDateStartFilter = o.CaseClosingDateStartFilter;
            r.CaseClosingDateEndFilter = o.CaseClosingDateEndFilter;
            r.CaseClosingReasonFilter = o.CaseClosingReasonFilter;
            r.ParentPathClosingReason = o.ParentPathClosingReason;
            r.CustomFilter = o.CustomFilter;
                   
            return r;
        }
    }
    
}

