namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class WorkingGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<WorkingGroupEntity> WorkingGroup { get; set; }

        public bool IsShowOnlyActive { get; set; }
    }
}