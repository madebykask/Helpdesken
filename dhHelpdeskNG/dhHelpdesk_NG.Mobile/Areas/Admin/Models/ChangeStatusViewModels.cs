namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;

    public class ChangeStatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeStatusEntity> ChangeStatuses { get; set; }
    }

    public class ChangeStatusInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeStatusEntity ChangeStatus { get; set; }
    }
}