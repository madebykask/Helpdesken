namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class SupplierInputViewModel
    {
        public Supplier Supplier { get; set; }
        public Customer Customer { get; set; }
        public IList<SelectListItem> Countries { get; set; }
    }
}