namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;    
    using DH.Helpdesk.Domain;
    using System.Web.Mvc;

    public class SelfServiceIndexViewModel
    {
        public Customer Customer { get; set; }

        public IList<SelectListItem> StartPageFAQNums { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> SelectedCategories { get; set; }
        public IList<SelectListItem> AvailableInitiators { get; set; }
        public IList<SelectListItem> SelectedInitiators { get; set; }
        public IList<SelectListItem> AvailableCaseTypes { get; set; }
        public IList<SelectListItem> SelectedCaseTypes { get; set; }
        public IList<SelectListItem> AvailableProductAreas { get; set; }
        public IList<SelectListItem> SelectedProductAreas { get; set; }

        public int CaseComplaintDays { get; set; }
    }

}