using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentTextEntity : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Text { get; set; }
        public string Headline { get; set; }
        public int SortOrder { get; set; }
        public int CaseDocumentParagraph_Id { get; set; }

        public virtual CaseDocumentParagraphEntity CaseDocumentParagraph { get; set; }
        public virtual ICollection<CaseDocumentTextConditionEntity> Conditions { get; set; }
    }
}
