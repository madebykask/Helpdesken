namespace DH.Helpdesk.Mobile.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class ChecklistActionInputViewModel
    {
        public ChecklistAction ChecklistAction { get; set; }

        public IList<SelectListItem> ChecklistServices { get; set; }
    }
}
