namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class QuickLinkInputViewModel
    {
        public Link Link { get; set; }
        public Customer Customer { get; set; }

        
        public IList<SelectListItem> Documents { get; set; }
        public IList<SelectListItem> LinkGroups { get; set; }

        public IList<SelectListItem> UsAvailable { get; set; }
        public IList<SelectListItem> UsSelected { get; set; }

        public IList<SelectListItem> CaseSolutions { get; set; }
        public IList<SelectListItem> FavoriteSearchFilters { get; set; }


        public IList<SelectListItem> WgAvailable { get; set; }
        public IList<SelectListItem> WgSelected { get; set; }
    }
}