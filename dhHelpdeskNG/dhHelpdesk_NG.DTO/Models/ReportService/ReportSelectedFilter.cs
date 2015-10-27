namespace DH.Helpdesk.BusinessData.Models.ReportService
{        
    using DH.Helpdesk.BusinessData.Models.Shared;    
    
    public class ReportSelectedFilter
    {
        public ReportSelectedFilter()
        {
            this.CaseCreationDate = new DateToDate();
            this.SelectedCustomers = new SelectedItems();
            this.SeletcedDepartments = new SelectedItems();
            this.SelectedWorkingGroups = new SelectedItems();
            this.SelectedAdministrator = new SelectedItems();
            this.SelectedCaseTypes = new SelectedItems();
        }

        public DateToDate CaseCreationDate { get; set; }

        public SelectedItems SelectedCustomers { get; set; }

        public SelectedItems SeletcedDepartments { get; set; }        

        public SelectedItems SelectedWorkingGroups { get; set; }

        public SelectedItems SelectedAdministrator { get; set; }

        public SelectedItems SelectedCaseTypes { get; set; }
                
    }
       
}