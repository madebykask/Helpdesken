namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class WorkingGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<WorkingGroupEntity> WorkingGroup { get; set; }
    }
}