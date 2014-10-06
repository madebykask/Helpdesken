namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class UrgencyIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Urgency> Urgencies { get; set; }
    }
}