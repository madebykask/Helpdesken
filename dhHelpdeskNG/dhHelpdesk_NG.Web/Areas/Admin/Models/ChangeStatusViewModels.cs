using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeStatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeStatusEntity> ChangeStatuses { get; set; }
    }

    public class ChangeStatusInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeStatusEntity ChangeStatus { get; set; }
    }
}