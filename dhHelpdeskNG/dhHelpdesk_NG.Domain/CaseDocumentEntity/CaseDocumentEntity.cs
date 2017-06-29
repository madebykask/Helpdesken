using System;

namespace DH.Helpdesk.Domain
{
    public class CaseDocumentEntity : EntityBase
    {
        public Guid CaseDocumentGUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Customer_Id { get; set; }
        public string FileType { get; set; }
        public int SortOrder { get; set; }
    }
}
