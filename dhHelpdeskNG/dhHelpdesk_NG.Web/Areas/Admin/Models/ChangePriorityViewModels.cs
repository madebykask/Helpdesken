using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChangePriorityIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangePriority> ChangePriorities { get; set; }
    }

    public class ChangePriorityInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangePriority ChangePriority { get; set; }
    }
}