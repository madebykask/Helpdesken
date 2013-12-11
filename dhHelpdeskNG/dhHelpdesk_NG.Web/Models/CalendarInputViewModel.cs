using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Models
{
    public class CalendarIndexViewModel
    {
        public string SearchCs { get; set; }

        public Calendar Calendar { get; set; }

        public IEnumerable<Calendar> Calendars { get; set; }
    }

    public class CalendarInputViewModel
    {
        public Calendar Calendar { get; set; }

        public IList<SelectListItem> WGsAvailable { get; set; }
        public IList<SelectListItem> WGsSelected { get; set; }

        public CalendarInputViewModel() { }
    }
}