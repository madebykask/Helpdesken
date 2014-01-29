using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeCategoryEntity> ChangeCategories { get; set; }
    }

    public class ChangeCategoryInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeCategoryEntity ChangeCategory { get; set; }
    }
}