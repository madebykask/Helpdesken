using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.Web.Models.Case
{
    public class CaseAttachedExFileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public bool IsCaseFile { get; set; }
        public int CaseId { get; set; }
        public int? LogId { get; set; }
        public LogFileType LogType { get; set; }
    }
}