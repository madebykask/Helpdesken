using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChangeCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeCategory> ChangeCategories { get; set; }
    }

    public class ChangeCategoryInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeCategory ChangeCategory { get; set; }
    }
}