namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class FloorInputViewModel 
    {
        public Floor Floor { get; set; }

        public IList<SelectListItem> Buildings { get; set; }
    }
}
