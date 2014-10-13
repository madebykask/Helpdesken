namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;

    public class ChangeImplementationStatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeImplementationStatusEntity> ChangeImplementationStatuses { get; set; }
    }

    public class ChangeImplementationStatusInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeImplementationStatusEntity ChangeImplementationStatus { get; set; }
    }
}