using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
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