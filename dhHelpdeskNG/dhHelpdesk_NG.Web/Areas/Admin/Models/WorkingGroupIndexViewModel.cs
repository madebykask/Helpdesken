using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class WorkingGroupIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<WorkingGroup> WorkingGroup { get; set; }
    }
}