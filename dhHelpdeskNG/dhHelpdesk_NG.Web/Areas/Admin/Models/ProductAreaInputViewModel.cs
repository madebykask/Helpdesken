using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ProductAreaInputViewModel
    {
        public ProductArea ProductArea { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> MailTemplates { get; set; }
        public IList<SelectListItem> WorkingGroups { get; set; }
    }
}