using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class StatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<Status> Statuses { get; set; }
    }
}