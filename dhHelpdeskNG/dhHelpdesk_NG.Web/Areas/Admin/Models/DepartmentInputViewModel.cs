using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
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