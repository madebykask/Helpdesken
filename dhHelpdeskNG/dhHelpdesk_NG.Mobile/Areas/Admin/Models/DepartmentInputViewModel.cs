namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class DepartmentInputViewModel
    {
        public Department Department { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> Countries { get; set; }
        public IList<SelectListItem> Regions { get; set; }
        public IList<SelectListItem> Holidays { get; set; }
        public IList<SelectListItem> WatchDateCalendar { get; set; }
    }
}