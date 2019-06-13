using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class CommonReportDataFilter
    {
        public int CustomerID { get; set; }
        public int? CaseStatus { get; set; }
        public DateTime? RegisterFrom { get; set; }
        public DateTime? RegisterTo { get; set; }
        public DateTime? CloseFrom { get; set; }
        public DateTime? CloseTo { get; set; }
        public List<int> Administrators { get; set; }
        public List<int> Departments { get; set; }
        public List<int> CaseTypes { get; set; }
        public List<int> ProductAreas { get; set; }
        public List<int> WorkingGroups { get; set; }
        public bool IncludeCasesWithNoWorkingGroup { get; set; }
        public bool IncludeCasesWithNoDepartments { get; set; }
        
    }
}