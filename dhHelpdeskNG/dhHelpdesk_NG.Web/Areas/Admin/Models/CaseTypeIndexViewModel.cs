using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class CaseTypeIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<CaseType> CaseTypes { get; set; }
    }
}