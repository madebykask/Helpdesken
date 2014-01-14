using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class SystemInputViewModel
    {
        public Domain.System System { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> Administrators { get; set; }
        public IList<SelectListItem> Suppliers { get; set; }
        public IList<SelectListItem> Urgencies { get; set; }
        public IList<SelectListItem> SystemResponsibleUsers { get; set; }
        public IList<SelectListItem> Domains { get; set; }
    }
}