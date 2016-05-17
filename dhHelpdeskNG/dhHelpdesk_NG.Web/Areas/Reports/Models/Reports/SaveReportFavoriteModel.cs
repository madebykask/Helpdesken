using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
    public class SaveReportFavoriteModel
    {
        public int? Id { get; set; }
        public int OriginalReportId { get; set; }
        public string Name { get; set; }
        public string Filters { get; set; }
    }

    public class ReportFavoriteModel
    {
        public int Id { get; set; }
        public int OriginalReportId { get; set; }
        public string Name { get; set; }
        public string Filters { get; set; }
    }
}