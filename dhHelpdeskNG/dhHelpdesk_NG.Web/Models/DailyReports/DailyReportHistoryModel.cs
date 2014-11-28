using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
namespace DH.Helpdesk.Web.Models.DailyReports
{
    public class DailyReportHistoryModel
    {
        public DailyReportHistoryModel()
        { }

        public DateTime ReportsFrom { get; set; }

        public DateTime? ReportsTo { get; set; }

        public List<DailyReportOverview> Reports { get; set; }
        
    }
}