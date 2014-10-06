namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class DomainIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Helpdesk.Domain.Domain> Domains { get; set; }
    }

    public class DomainInputViewModel
    {
        public Customer Customer { get; set; }
        public Helpdesk.Domain.Domain Domain { get; set; }

        public string ConfirmPassword { get; set; }
        public string NewPassword { get; set; }
    }
}