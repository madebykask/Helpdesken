using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ContractCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ContractCategory> ContractCategories { get; set; }
    }
}