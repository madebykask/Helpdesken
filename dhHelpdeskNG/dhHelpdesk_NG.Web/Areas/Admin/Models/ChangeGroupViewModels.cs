namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;

    public class ChangeGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeGroupEntity> ChangeGroups { get; set; }
    }

    public class ChangeGroupInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeGroupEntity ChangeGroup { get; set; }
    }
}