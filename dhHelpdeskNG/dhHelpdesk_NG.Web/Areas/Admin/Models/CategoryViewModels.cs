namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class CategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Category> Categories { get; set; }
        public bool IsShowOnlyActive { get; set; }
    }

    public class CategoryInputViewModel
    {
        public Customer Customer { get; set; }
        public Category Category { get; set; }
    }
}