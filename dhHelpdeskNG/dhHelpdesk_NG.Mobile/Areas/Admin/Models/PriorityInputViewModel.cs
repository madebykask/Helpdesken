namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class PriorityInputViewModel
    {
        public Priority Priority { get; set; }
        public Customer Customer { get; set; }
        public PriorityLanguage PriorityLanguage { get; set; }

        public IList<SelectListItem> EmailTemplates { get; set; }
        public IList<SelectListItem> Languages { get; set; }
    }
}