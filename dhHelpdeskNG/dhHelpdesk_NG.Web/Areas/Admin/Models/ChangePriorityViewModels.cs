namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;

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