using System.Collections.Generic;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChangeStatusIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeStatus> ChangeStatuses { get; set; }
    }

    public class ChangeStatusInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeStatus ChangeStatus { get; set; }
    }
}