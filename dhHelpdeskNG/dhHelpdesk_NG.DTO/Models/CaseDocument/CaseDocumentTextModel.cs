using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Shared.Input;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentTextModel: INewBusinessModel
    {
        public CaseDocumentTextModel()
        {
        }

        public int Id { get; set; }
        public int CaseDocumentParagraph_Id { get; set; }
        public int SortOrder { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Headline { get; set; }
        public string Text { get; set; } 
        
        public IList<CaseDocumentTextConditionModel> Conditions { get; set; }
    }
}