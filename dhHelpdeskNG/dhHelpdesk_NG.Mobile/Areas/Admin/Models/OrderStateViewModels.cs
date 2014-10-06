namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

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