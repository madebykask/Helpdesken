namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class QuickLinkIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Link> Links { get; set; }
    }
}