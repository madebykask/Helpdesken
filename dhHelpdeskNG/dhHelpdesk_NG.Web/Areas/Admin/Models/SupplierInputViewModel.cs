using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class SupplierInputViewModel
    {
        public Supplier Supplier { get; set; }
        public Customer Customer { get; set; }
        public IList<SelectListItem> Countries { get; set; }
    }
}