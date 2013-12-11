using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class OrderStateIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<OrderState> OrderStates { get; set; }
    }

    public class OrderStateInputViewModel
    {
        public Customer Customer { get; set; }
        public OrderState OrderState { get; set; }
    }
}