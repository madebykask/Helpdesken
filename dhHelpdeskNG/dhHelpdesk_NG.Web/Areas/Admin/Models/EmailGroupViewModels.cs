namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class EmailGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<EmailGroupEntity> EmailGroups { get; set; }
        public bool IsShowOnlyActive { get; set; }
    }

    public class EmailGroupInputViewModel
    {
        public Customer Customer { get; set; }
        public EmailGroupEntity EmailGroup { get; set; }
    }
}