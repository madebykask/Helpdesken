using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ImpactIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Impact> Impacts { get; set; }
    }
}