using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Models
{
    public class DocumentInputViewModel
    {
        public Document Document { get; set; }

        public IList<Document> Documents { get; set; }
        public IList<DocumentCategory> DocumentCategories { get; set; }

        public IList<SelectListItem> DocumentCats { get; set; }

        public IList<SelectListItem> UsAvailable { get; set; }
        public IList<SelectListItem> UsSelected { get; set; }
        public IList<SelectListItem> WGsAvailable { get; set; }
        public IList<SelectListItem> WGsSelected { get; set; }

        public DocumentInputViewModel() { }
    }
}