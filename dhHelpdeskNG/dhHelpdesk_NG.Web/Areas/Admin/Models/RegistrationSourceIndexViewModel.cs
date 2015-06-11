namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class RegistrationSourceCustomerIndex
    {
        public int Id { get; set; }

        public string SourceName { get; set; }

        public string SystemCodeName { get; set; }

        public string IsActive { get; set; }
    }

    public class RegistrationSourceIndexViewModel
    {
        public Customer Customer { get; set; }

        public IList<RegistrationSourceCustomerIndex> RegistrationSources { get; set; }
    }
}
