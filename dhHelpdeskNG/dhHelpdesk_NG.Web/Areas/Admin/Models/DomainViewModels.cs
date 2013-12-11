using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class DomainIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Domain.Domain> Domains { get; set; }
    }

    public class DomainInputViewModel
    {
        public Customer Customer { get; set; }
        public Domain.Domain Domain { get; set; }
    }
}