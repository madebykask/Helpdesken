using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Models.Case.Logs
{
    public class CaseLogOutputModel : CaseBaseEventModel
    {
        public string Text { get; set; }
        public bool IsExternal { get; set; }
        public string RegUserName { get; set; }
        public IList<string> Emails { get; set; }
        public IList<LogFileModel> Files { get; set; }
    }

    public class LogFileModel
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CaseId { get; set; }
    }
}
