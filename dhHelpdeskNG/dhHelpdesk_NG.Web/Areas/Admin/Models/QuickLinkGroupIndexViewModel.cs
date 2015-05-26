namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class QuickLinkGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<LinkGroup> LinkGroups { get; set; }
    }
}