using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangePriorityIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangePriorityEntity> ChangePriorities { get; set; }
    }

    public class ChangePriorityInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangePriorityEntity ChangePriority { get; set; }
    }
}