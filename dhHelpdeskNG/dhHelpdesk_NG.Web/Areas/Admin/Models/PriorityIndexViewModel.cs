using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class PriorityIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Priority> Priorities { get; set; }
    }
}