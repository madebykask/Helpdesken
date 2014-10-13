namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class CategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Category> Categories { get; set; }
    }

    public class CategoryInputViewModel
    {
        public Customer Customer { get; set; }
        public Category Category { get; set; }
    }
}