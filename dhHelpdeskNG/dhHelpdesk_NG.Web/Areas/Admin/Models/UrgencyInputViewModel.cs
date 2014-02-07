namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class UrgencyInputViewModel
    {
        public Urgency Urgency { get; set; }
        public Customer Customer { get; set; }

        public IList<Impact> Impacts { get; set; }
        public IList<Priority> Priorities { get; set; }
        public IList<Urgency> Urgencies { get; set; }
    }
}