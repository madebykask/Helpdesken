namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class DailyReportSubjectInputViewModel
    {
        public int ShowYesNo { get; set; }
        public int StartPageShow { get; set; }

        public DailyReportSubject DailyReportSubject { get; set; }
        public Customer Customer { get; set; }
        public List<SelectListItem> NumberToShowOnStartPage { get; set; }
    }
}