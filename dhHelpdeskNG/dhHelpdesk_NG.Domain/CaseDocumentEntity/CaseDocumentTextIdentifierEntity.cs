using System;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentTextIdentifierEntity : Entity
    {
        public int ExtendedCaseFormId { get; set; }
        public string Identifier { get; set; }
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        public string DisplayFormat { get; set; }
        public string DataFormat { get; set; }
        public string DataType { get; set; }
    }
}
