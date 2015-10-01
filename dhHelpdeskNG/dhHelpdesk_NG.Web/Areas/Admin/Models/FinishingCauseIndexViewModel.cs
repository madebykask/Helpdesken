namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class FinishingCauseIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<FinishingCause> FinishingCauses { get; set; }

        public bool IsShowOnlyActive { get; set; }
    }
}