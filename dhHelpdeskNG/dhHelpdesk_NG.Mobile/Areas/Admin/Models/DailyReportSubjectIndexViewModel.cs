namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class DailyReportSubjectIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<DailyReportSubject> DailyReportSubjects { get; set; }
    }
}