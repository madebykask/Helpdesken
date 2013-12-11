using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class UrgencyInputViewModel
    {
        public Urgency Urgency { get; set; }
        public Customer Customer { get; set; }

        public IList<Impact> Impacts { get; set; }
        public IList<Priority> Priorities { get; set; }
        public IList<Urgency> Urgencies { get; set; }
    }
}