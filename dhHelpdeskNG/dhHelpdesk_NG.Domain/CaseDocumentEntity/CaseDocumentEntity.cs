using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentEntity : EntityBase
    {
        public Guid CaseDocumentGUID { get; set; }
        public int CaseDocumentTemplate_Id { get; set; }
        public int Customer_Id { get; set; }
        public string FileType { get; set; }

        public virtual ICollection<CaseDocumentConditionEntity> Conditions { get; set; }
        public virtual ICollection<CaseDocument_CaseDocumentParagraphEntity> CaseDocumentParagraphsKeys { get; set; }
        public virtual CaseDocumentTemplateEntity CaseDocumentTemplate { get; set; }
    }
}
