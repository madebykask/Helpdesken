using System;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentConditionEntity : EntityBase
    {
        public int CaseDocument_Id { get; set; }
        public Guid CaseDocumentConditionGUID { get; set; }
        public string Property_Name { get; set; }
        public string Values { get; set; }
        public string Description { get; set; }
    }
}
