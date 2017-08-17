using DH.Helpdesk.BusinessData.Models.Shared.Input;
using System;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentTextConditionIdentifierModel : INewBusinessModel
    {
        public CaseDocumentTextConditionIdentifierModel()
        {
        }

        public int Id { get; set; }
        public int ExtendedCaseFormId { get; set; }
        public string Identifier { get; set; }
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
    }
}