namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class OrderTypeInputViewModel
    {
        public OrderType OrderType { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> CaseTypes { get; set; }
        public IList<SelectListItem> Documents { get; set; }
        public IList<SelectListItem> ParentOrderType { get; set; }
    }
}