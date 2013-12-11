using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class OUIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<OU> OUs { get; set; }
    }
}