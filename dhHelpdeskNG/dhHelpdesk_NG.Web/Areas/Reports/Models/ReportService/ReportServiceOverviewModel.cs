namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{
    using System.IO;
    using System.Collections.Generic;
    
    using DH.Helpdesk.BusinessData.Models.ReportService;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using Microsoft.Reporting.WebForms;

    public sealed class ReportServiceOverviewModel
    {   
        public ReportServiceOverviewModel()
        {
            
        }

        public CustomSelectList ReportList { get; set; }

        public ReportFilterModel ReportFilter { get; set; }
        
        public ReportViewer ReportViewerData { get; set; }
        
    }

    public sealed class ReportFilterModel : ReportFilter
    {
        public ReportSelectedFilter Selected { get; set; }

        public string CaseTypeSelectedText { get; set; }

        public int UserOrientationName { get; set; }
    }
       
}