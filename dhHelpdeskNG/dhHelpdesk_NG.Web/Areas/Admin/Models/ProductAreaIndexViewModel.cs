using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ProductAreaIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ProductArea> ProductAreas { get; set; }
    }
}