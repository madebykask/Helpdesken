namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class SupplierIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Supplier> Suppliers { get; set; }
    }
}