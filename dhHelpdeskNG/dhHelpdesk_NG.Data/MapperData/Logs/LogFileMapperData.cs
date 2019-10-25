
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.Dal.MapperData.Logs
{
    public class LogFileMapperData
    {
        public int? Id { get; set; }
        public string FileName { get; set; }
        public int? CaseId { get; set; }
        public int? LogId { get; set; }
        public LogFileType? LogType { get; set; }
        public LogFileType? ParentLogType { get; set; }
    }
}