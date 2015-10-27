using DH.Helpdesk.BusinessData.Models.ReportService;
using DH.Helpdesk.BusinessData.Models.Shared;
using System;
namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{        
    public class ReportServiceSessionModel
    {
        public ReportServiceSessionModel()
        {
            
        }

        public string ReportName { get; set; }

        public ReportSelectedFilter SelectedFilter { get; set; }
        
    }

}