using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class FinishingCauseCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<FinishingCauseCategory> FinishingCauseCategories { get; set; }
    }
}