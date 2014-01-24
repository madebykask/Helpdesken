using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeGroupEntity> ChangeGroups { get; set; }
    }

    public class ChangeGroupInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeGroupEntity ChangeGroup { get; set; }
    }
}