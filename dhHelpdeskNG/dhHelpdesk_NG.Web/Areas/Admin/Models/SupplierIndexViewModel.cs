namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class SupplierIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Supplier> Suppliers { get; set; }
        public bool IsShowOnlyActive { get; set; }
    }
}