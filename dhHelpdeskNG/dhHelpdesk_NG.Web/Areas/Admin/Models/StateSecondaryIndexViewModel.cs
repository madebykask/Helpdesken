namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class StateSecondaryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<StateSecondary> StateSecondaries { get; set; }
        public bool IsShowOnlyActive { get; set; }
    }
}