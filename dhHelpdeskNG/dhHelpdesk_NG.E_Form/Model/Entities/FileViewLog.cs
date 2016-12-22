using System;

namespace ECT.Model.Entities
{
    public class FileViewLog
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileSource { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
