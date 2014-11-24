using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
namespace DH.Helpdesk.Web.Models.DailyReports
{
    public class DailyReportOutputModel
    {
        public List<DailyReportSubject> Subjects { get; set; }
        public IEnumerable<DailyReportOverview> Reports { get; set; }

    }

    public class HistoryInputViewModel
    {
        /// <summary>
        /// Gets or sets the  From.
        /// </summary>
        public DateTime ReportsFrom { get; set; }

        /// <summary>
        /// Gets or sets the  From.
        /// </summary>
        public DateTime ReportsTo { get; set; }

        /// <summary>
        /// Gets or sets the subject selected.
        /// </summary>
        public IList<DailyReportSubject> SSelected { get; set; }

        public HistoryInputViewModel() { }
    }
}