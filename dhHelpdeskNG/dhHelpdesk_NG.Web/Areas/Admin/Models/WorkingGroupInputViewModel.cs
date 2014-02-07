namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class WorkingGroupInputViewModel
    {
        public WorkingGroupEntity WorkingGroup { get; set; }
        public Customer Customer { get; set; }
        public IList<UserWorkingGroup> UsersForWorkingGroup { get; set; }

        public IList<SelectListItem> StateSecondaries { get; set; }
    }
}