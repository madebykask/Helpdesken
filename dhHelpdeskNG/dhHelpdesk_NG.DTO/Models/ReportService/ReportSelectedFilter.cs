namespace DH.Helpdesk.BusinessData.Models.ReportService
{        
    using DH.Helpdesk.BusinessData.Models.Shared;
using System.Collections.Generic;    
    
    public class ReportSelectedFilter
    {
        public ReportSelectedFilter()
        {
            this.CaseCreationDate = new DateToDate();
            this.SelectedCustomers = new SelectedItems();
            this.SeletcedDepartments = new SelectedItems();
            this.SeletcedOUs = new SelectedItems();
            this.SelectedWorkingGroups = new SelectedItems();
            this.SelectedAdministrator = new SelectedItems();
            this.SelectedCaseTypes = new SelectedItems();
            this.SelectedProductAreas = new SelectedItems();
            this.SelectedCaseStatus = new SelectedItems();
            this.GeneralParameter = new List<GeneralParameter>();
            this.CaseClosingDate = new DateToDate();
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