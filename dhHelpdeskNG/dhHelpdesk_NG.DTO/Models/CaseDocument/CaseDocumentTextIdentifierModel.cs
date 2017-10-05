using DH.Helpdesk.BusinessData.Models.Shared.Input;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
    public class CaseDocumentTextIdentifierModel : INewBusinessModel
    {
        public CaseDocumentTextIdentifierModel()
        {
        }

        public int Id { get; set; }
        public int ExtendedCaseFormId { get; set; }
        public string Identifier { get; set; }
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        public string DisplayFormat { get; set; }
        public string DataFormat { get; set; }
        public string DataType { get; set; }
    }
}