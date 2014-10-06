namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;

    public class ChangeObjectIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeObjectEntity> ChangeObjects { get; set; }
    }

    public class ChangeObjectInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeObjectEntity ChangeObject { get; set; }
    }
}