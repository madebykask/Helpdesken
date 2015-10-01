namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class DailyReportSubjectIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<DailyReportSubject> DailyReportSubjects { get; set; }

        public bool IsShowOnlyActive { get; set; }
    }
}