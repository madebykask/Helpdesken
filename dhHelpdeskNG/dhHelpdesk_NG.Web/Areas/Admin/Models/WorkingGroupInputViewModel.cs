using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class WorkingGroupInputViewModel
    {
        public WorkingGroupEntity WorkingGroup { get; set; }
        public Customer Customer { get; set; }
        public IList<UserWorkingGroup> UsersForWorkingGroup { get; set; }

        public IList<SelectListItem> StateSecondaries { get; set; }
    }
}