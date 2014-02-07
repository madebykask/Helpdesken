namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class StatusInputViewModel
    {
        public Status Status { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> WorkingGroups { get; set; }
        public IList<SelectListItem> StateSecondary { get; set; }
    }
}