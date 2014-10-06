namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class PriorityIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Priority> Priorities { get; set; }
    }
}