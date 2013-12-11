using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class QuickLinkIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Link> Links { get; set; }
    }
}