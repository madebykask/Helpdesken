using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{
    public class HistoricalReportFilterModel: CommonReportFilterModel
    {
        public int GroupBy { get; set; }
        public int StackBy { get; set; }
        public DateTime? HistoricalChangeDateFrom { get; set; }
        public DateTime? HistoricalChangeDateTo { get; set; }
        public List<int> HistoricalWorkingGroups { get; set; }
    }
}