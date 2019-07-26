using System;
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.Domain
{
    public class LogFileExisting : Entity
    {
        public int? Log_Id { get; set; }
        public int Case_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public LogFileType LogType { get; set; }
        public bool IsInternalLogNote { get; set; }
    }
}
