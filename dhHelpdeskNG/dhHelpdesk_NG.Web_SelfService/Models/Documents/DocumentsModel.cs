using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Models.Documents
{    
    using DH.Helpdesk.Domain;            

    public class DocumentsModel 
    {
        public DocumentsModel()
        {             

        }

        public string BaseFilePath { get; set; }

        public List<Document> Documents { get; set; }
                       
    }

}
