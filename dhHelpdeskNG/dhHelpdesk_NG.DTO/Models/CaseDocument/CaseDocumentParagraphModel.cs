using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentParagraphModel: INewBusinessModel
    {
        public CaseDocumentParagraphModel()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParagraphType { get; set; } //TODO: make enum of this 1, =

         public  ICollection<DH.Helpdesk.Domain.CaseDocumentTextEntity> CaseDocumentTexts { get; set; }
        // public ICollection<CaseDocumentParagraphModel> CaseDocumentParagraphs { get; set; }

        ////TODO: place below in base class model so we dont need to add this on all places
        //public int Status { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int? CreatedByUser_Id { get; set; }
        //public DateTime ChangedDate { get; set; }
        //public int? ChangedByUser_Id { get; set; }
    }
}