namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using BusinessData.Models.Customer;

    public class DepartmentInputViewModel
    {
        public Department Department { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> Countries { get; set; }
        public IList<SelectListItem> Regions { get; set; }
        public IList<SelectListItem> Holidays { get; set; }
        public IList<SelectListItem> WatchDateCalendar { get; set; }

        public CustomerSettings CustomerSettings { get; set; }

        public IList<SelectListItem> Languages { get; set; }
        public IList<SelectListItem> InvoiceAvailableOUs { get; set; }
        public IList<SelectListItem> InvoiceSelectedOUs { get; set; }

    }
}