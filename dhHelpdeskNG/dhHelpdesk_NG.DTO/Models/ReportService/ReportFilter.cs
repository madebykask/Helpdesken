
namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    using System.Collections.Generic;
    using DH.Helpdesk.Domain;    
    using DH.Helpdesk.BusinessData.Models.Shared;

    public class ReportFilter
    {
        public ReportFilter()
        {
        }

        public DateToDate CaseCreationDate { get; set; }

        public DateToDate CaseClosingDate { get; set; }
        public DateToDate LogNoteDate { get; set; }

        public List<User> Administrators { get; set; }

        public List<Department> Departments { get; set; }

        public List<WorkingGroupEntity> WorkingGroups { get; set; }

        public List<CaseType> CaseTypes { get; set; }

        public List<ProductArea> ProductAreas { get; set; }

        public CustomSelectList Status { get; set; }

        public List<ListItem> ReportCategory { get; set; }
        public List<ListItem> ReportCategoryRt { get; set; }
        public List<ListItem> ReportCategorySolvedInTime { get; set; }
    }

}