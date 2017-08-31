using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentTemplateEntity : Entity
    {
        public string Name { get; set; }
        public bool PageNumbersUse { get; set; }
        public Guid CaseDocumentTemplateGUID { get; set; }
        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }
        public int FooterHeight { get; set; }
        public int HeaderHeight { get; set; }

        public virtual ICollection<CaseDocumentEntity> CaseDocuments { get; set; }
    }
}
