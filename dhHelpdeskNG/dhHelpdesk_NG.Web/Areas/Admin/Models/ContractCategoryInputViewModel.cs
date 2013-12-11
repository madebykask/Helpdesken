using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ContractCategoryInputViewModel
    {
        public ContractCategory ContractCategory { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> CaseType { get; set; }
        public IList<SelectListItem> StateSecondary { get; set; }
    }
}