using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentParagraphEntity : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParagraphType { get; set; }

        public virtual ICollection<CaseDocumentTextEntity> CaseDocumentTexts { get; set; }
    }
}
