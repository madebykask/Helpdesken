using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class SystemIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Domain.System> System { get; set; }
    }
}