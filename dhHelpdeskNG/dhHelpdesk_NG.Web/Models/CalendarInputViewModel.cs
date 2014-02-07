namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

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