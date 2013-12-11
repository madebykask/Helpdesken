using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class OrderTypeIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<OrderType> OrderTypes { get; set; }
    }
}