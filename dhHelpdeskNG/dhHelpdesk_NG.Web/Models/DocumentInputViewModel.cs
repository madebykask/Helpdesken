using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

   
    public class DocumentInputViewModel
    {
        public DocumentInputViewModel()
        {
            
        }

        public Document Document { get; set; }

        public IList<Document> Documents { get; set; }

        public IList<DocumentCategory> DocumentCategories { get; set; }

        public IList<SelectListItem> DocumentCats { get; set; }

        public IList<SelectListItem> UsAvailable { get; set; }

        public IList<SelectListItem> UsSelected { get; set; }

        public IList<SelectListItem> WGsAvailable { get; set; }

        public IList<SelectListItem> WGsSelected { get; set; }        

        public TreeContent DocumentTree { get; set; }
    }
}