using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeImplementationStatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeImplementationStatusEntity> ChangeImplementationStatuses { get; set; }
    }

    public class ChangeImplementationStatusInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeImplementationStatusEntity ChangeImplementationStatus { get; set; }
    }
}