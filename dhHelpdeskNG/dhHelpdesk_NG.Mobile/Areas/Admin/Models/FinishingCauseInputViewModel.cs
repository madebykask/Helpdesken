namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class FinishingCauseInputViewModel
    {
        public FinishingCause FinishingCause { get; set; }
        public Customer Customer { get; set; }
        public IList<SelectListItem> FinishingCauseCategories { get; set; }
    }
}