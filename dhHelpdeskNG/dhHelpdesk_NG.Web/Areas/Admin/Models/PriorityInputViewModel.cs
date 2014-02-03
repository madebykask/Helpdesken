using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class PriorityInputViewModel
    {
        public Priority Priority { get; set; }
        public Customer Customer { get; set; }
        public PriorityLanguage PriorityLanguage { get; set; }

        public IList<SelectListItem> EmailTemplates { get; set; }
        public IList<SelectListItem> Languages { get; set; }
    }
}