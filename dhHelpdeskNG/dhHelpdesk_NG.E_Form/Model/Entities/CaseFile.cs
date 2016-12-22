using System;

namespace ECT.Model.Entities
{
    public class CaseFile
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
