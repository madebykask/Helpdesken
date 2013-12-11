using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChangeGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeGroup> ChangeGroups { get; set; }
    }

    public class ChangeGroupInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeGroup ChangeGroup { get; set; }
    }
}