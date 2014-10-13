namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class SystemInputViewModel
    {
        public System System { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> Administrators { get; set; }
        public IList<SelectListItem> Suppliers { get; set; }
        public IList<SelectListItem> Urgencies { get; set; }
        public IList<SelectListItem> SystemResponsibleUsers { get; set; }
        public IList<SelectListItem> Domains { get; set; }
        public IList<SelectListItem> OperatingSystem { get; set; }
    }
}