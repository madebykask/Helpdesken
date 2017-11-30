namespace DH.Helpdesk.BusinessData.Models.ReportService
{        
    using DH.Helpdesk.BusinessData.Models.Shared;
    using System.Collections.Generic;    
    
    public class ReportSelectedFilter
    {
        public ReportSelectedFilter()
        {
            CaseCreationDate = new DateToDate();
            SelectedCustomers = new SelectedItems();
            SeletcedDepartments = new SelectedItems();
            SeletcedOUs = new SelectedItems();
            SelectedWorkingGroups = new SelectedItems();
            SelectedAdministrator = new SelectedItems();
            SelectedCaseTypes = new SelectedItems();
            SelectedProductAreas = new SelectedItems();
            SelectedCaseStatus = new SelectedItems();
            GeneralParameter = new List<GeneralParameter>();
            SelectedReportCategory = new SelectedItems();
            SelectedReportCategoryRt = new SelectedItems();
            CaseClosingDate = new DateToDate();
        }

        public int CaseId { get; set; }

        public int LanguageId { get; set; }

        public DateToDate CaseCreationDate { get; set; }

        public DateToDate CaseClosingDate { get; set; }

        public SelectedItems SelectedCustomers { get; set; }

        public SelectedItems SeletcedDepartments { get; set; }

        public SelectedItems SeletcedOUs { get; set; }        

        public SelectedItems SelectedWorkingGroups { get; set; }

        public SelectedItems SelectedAdministrator { get; set; }

        public SelectedItems SelectedCaseTypes { get; set; }

        public SelectedItems SelectedProductAreas { get; set; }

        public SelectedItems SelectedCaseStatus { get; set; }
                
        public List<GeneralParameter> GeneralParameter {get; set;}

        public SelectedItems SelectedReportCategory { get; set; }
        public SelectedItems SelectedReportCategoryRt { get; set; }

    }
       
    public class GeneralParameter 
    {
        public GeneralParameter(string paramName, object paramValue)
        {
            ParamName = paramName;
            ParamValue = paramValue;
        }

        public string ParamName {get; set;}

        public object ParamValue {get; set;}

    }
}