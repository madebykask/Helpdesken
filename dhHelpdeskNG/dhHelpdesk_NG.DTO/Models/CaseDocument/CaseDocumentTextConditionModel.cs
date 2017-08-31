using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentTextConditionModel : INewBusinessModel
    {
        public CaseDocumentTextConditionModel()
        {
        }

        public int Id { get; set; }
        public int CaseDocumentText_Id { get; set; }
        public Guid CaseDocumentTextConditionGUID { get; set; }
        public string Property_Name { get; set; }
        public string Values { get; set; }
        public string Operator { get; set; }    //TODO:, make enum
        public string Description { get; set; }

        //TODO: place below in base class model so we dont need to add this on all places
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedByUser_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public int? ChangedByUser_Id { get; set; }
    }
}