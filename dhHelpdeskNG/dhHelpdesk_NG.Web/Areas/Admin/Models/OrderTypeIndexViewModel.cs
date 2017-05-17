namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class OrderTypeIndexViewModel
    {
        public Customer Customer { get; set; }
        public bool IsShowOnlyActive { get; set; }
        public IList<OrderType> OrderTypes { get; set; }
    }
}