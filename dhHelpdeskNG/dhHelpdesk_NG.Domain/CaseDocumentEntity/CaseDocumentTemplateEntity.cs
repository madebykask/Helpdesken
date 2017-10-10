using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentTemplateEntity : Entity
    {
        public string Name { get; set; }
        public Guid CaseDocumentTemplateGUID { get; set; }
        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }
        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }
        public double FooterHeight { get; set; }
        public double HeaderHeight { get; set; }

        public int ShowFooterFromPageNr { get; set; }
        public int ShowHeaderFromPageNr { get; set; }
        public string Style { get; set; }

        public bool ShowAlternativeHeaderOnFirstPage { get; set; }
        public bool ShowAlternativeFooterOnFirstPage { get; set; }
        public double DraftHeight { get; set; }
        public double DraftYLocation { get; set; }
        public double DraftRotateAngle { get; set; }
        public int HtmlViewerWidth { get; set; }

        public virtual ICollection<CaseDocumentEntity> CaseDocuments { get; set; }
    }
}

