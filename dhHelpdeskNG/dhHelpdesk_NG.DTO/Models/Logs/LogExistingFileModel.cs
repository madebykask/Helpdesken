
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.BusinessData.Models.Logs
{
    public class LogExistingFileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsExistCaseFile { get; set; }
        public bool IsExistLogFile { get; set; }
        public int? LogId { get; set; }
        public int CaseId { get; set; }
        public LogFileType LogType { get; set; }
        public bool IsInternalLogNote { get; set; }
    }
}
