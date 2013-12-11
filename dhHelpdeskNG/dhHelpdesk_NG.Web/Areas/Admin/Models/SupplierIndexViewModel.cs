using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class SupplierIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Supplier> Suppliers { get; set; }
    }
}