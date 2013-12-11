using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ChecklistActionInputViewModel
    {
        public ChecklistAction ChecklistAction { get; set; }

        public IList<SelectListItem> ChecklistServices { get; set; }
    }
}
