using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class StateSecondaryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<StateSecondary> StateSecondaries { get; set; }
    }
}