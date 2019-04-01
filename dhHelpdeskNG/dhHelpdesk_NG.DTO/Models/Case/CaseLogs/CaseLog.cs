using System;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseLogs
{
    public class CaseLogData
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserSurName { get; set; }
        public string RegUserName { get; set; }
        public string InternalText { get; set; }
        public string ExternalText { get; set; }
        public DateTime LogDate { get; set; }
        public IList<string> Emails { get; set; }
        public IList<LogFileData> Files { get; set; }
    }

    public class LogFileData
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int LogId { get; set; }
        public int? CaseId { get; set; }

    }
}