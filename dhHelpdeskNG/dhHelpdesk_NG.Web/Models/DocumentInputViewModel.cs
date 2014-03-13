using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

namespace DH.Helpdesk.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    

    public class DocumentOverview
    {
        private int p1;
        private string p2;
        private int p3;
        private DateTime dateTime;
        private string p4;

        public DocumentOverview(int id, string docName, int size, DateTime changedDate, string userName)
        {
            // TODO: Complete member initialization
            this.Id = id;
            this.DocName = docName;
            this.Size = size;
            this.ChangedDate = changedDate;
            this.UserName = userName;
        }

        public int Id { get; set; }

        public string DocName { get; set; }

        public int Size { get; set; }

        public DateTime ChangedDate { get; set; }

        public string UserName { get; set; }

    }


    public class DocumentInputViewModel
    {
        public DocumentInputViewModel()
        {
            
        }

        public int CurrentDocType { get; set; }

        public string CurrentItemName { get; set; }

        public Document Document { get; set; }

        public IList<DocumentOverview> Documents { get; set; }

        public IList<DocumentCategory> DocumentCategories { get; set; }

        public IList<SelectListItem> DocumentCats { get; set; }

        public IList<SelectListItem> UsAvailable { get; set; }

        public IList<SelectListItem> UsSelected { get; set; }

        public IList<SelectListItem> WGsAvailable { get; set; }

        public IList<SelectListItem> WGsSelected { get; set; }        

        public TreeContent DocumentTree { get; set; }
    }
}