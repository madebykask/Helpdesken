using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DH.Helpdesk.NewSelfService.Models.Documents
{    
    using DH.Helpdesk.Domain;            

    public class DocumentsModel 
    {
        public DocumentsModel()
        {             
        }
        
        public List<Document> Documents { get; set; }
                       
    }

}
