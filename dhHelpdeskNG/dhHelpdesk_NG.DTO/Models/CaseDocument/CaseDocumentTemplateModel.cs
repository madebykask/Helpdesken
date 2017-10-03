using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentTemplateModel : INewBusinessModel
    {
        public CaseDocumentTemplateModel()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool PageNumbersUse { get; set; }
        public Guid CaseDocumentTemplateGUID { get; set; }
        public int MarginTop { get; set; }
        public int MarginBottom { get; set; }
        public int MarginLeft { get; set; }
        public int MarginRight { get; set; }
        public int FooterHeight { get; set; }
        public int HeaderHeight { get; set; }
        public int ShowFooterFromPageNr { get; set; }
        public int ShowHeaderFromPageNr { get; set; }
        public string Style { get; set; }
    }
}