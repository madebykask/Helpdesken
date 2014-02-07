namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class StateSecondaryInputViewModel
    {
        public StateSecondary StateSecondary { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> WorkingGroups { get; set; }
    }
}