using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChangeImplementationStatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeImplementationStatus> ChangeImplementationStatuses { get; set; }
    }

    public class ChangeImplementationStatusInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeImplementationStatus ChangeImplementationStatus { get; set; }
    }
}