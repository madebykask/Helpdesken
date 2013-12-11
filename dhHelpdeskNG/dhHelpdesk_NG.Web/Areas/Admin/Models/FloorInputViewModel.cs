using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class FloorInputViewModel 
    {
        public Floor Floor { get; set; }

        public IList<SelectListItem> Buildings { get; set; }
    }
}
