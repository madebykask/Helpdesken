using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;
using System.Collections.Generic;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentModel: INewBusinessModel
    {
        public int Id { get; set; }
        public int CaseDocumentTemplate_Id { get; set; }
        public Guid CaseDocumentGUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Customer_Id { get; set; }
        public string FileType { get; set; }
        public int SortOrder { get; set; }
        public int CaseId { get; set; }

        //TODO: place below in base class model so we dont need to add this on all places
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public int? ChangedByUser_Id { get; set; }

        public IList<CaseDocumentParagraphModel> CaseDocumentParagraphs { get; set; }

        //todo: replace with model
        public CaseDocumentTemplateEntity CaseDocumentTemplate { get; set; }


		// Invalid Fields
		public string Errors { get; set; }
    }
  
}