using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class CaseTypeInputViewModel
    {
        public CaseType CaseType { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> SystemOwners { get; set; }
    }
}