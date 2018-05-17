using System;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentTextConditionEntity : EntityBase
    {
        public int CaseDocumentText_Id { get; set; }
        public Guid CaseDocumentTextConditionGUID { get; set; }
        public string Property_Name { get; set; }
        public string Values { get; set; }
        public string Operator { get; set; } //TODO: make enum
    }
}
