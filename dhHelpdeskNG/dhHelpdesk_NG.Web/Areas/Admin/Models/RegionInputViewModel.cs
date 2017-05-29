namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;
    using System.Web.Mvc;    
    


    public class RegionInputViewModel
    {
        public Region Region { get; set; }
        public Customer Customer { get; set; }

        public IList<SelectListItem> Languages { get; set; }

    }
}
