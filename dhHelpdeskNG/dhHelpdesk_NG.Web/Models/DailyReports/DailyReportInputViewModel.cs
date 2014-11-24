using DH.Helpdesk.BusinessData.Models.DailyReport.Input;
using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.DailyReports
{
    public class DailyReportInputViewModel
    {
        public DailyReportOverview DailyReport { get; set; }
        public DailyReportUpdate NewDailyreport { get; set; }

        public DailyReportInputViewModel() { }
    }
}