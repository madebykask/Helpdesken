using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class StateSecondaryInputViewModel
    {
        public StateSecondary StateSecondary { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> WorkingGroups { get; set; }
    }
}