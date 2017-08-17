using System;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentParagraphConditionEntity : EntityBase
    {
        public int CaseDocument_Id { get; set; }
        public Guid CaseDocumentParagraphConditionGUID { get; set; }
        public int CaseDocumentParagraph_Id { get; set; }
        public string Property_Name { get; set; }
        public string Values { get; set; }
        public string Operator { get; set; } //TODO: make enum
        public string Description { get; set; }
        
    }
}
