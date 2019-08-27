using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Common.Enums.Logs;

namespace DH.Helpdesk.SelfService.Models.Case
{
    public class CaseLogsModel 
    {
        public CaseLogsModel()
        { 
        }

        public CaseLogsModel(int caseId, 
            List<CaseLogModel> caseLogs, 
            string currentUser, 
            bool showInteralLog,
            bool isAttachmentsAllowed) 
        {
            CaseId = caseId;
            CurrentUser = currentUser;
            CaseLogs = caseLogs;
            ShowInternalLogNotes = showInteralLog;
            IsAttachmentsAllowed = isAttachmentsAllowed;
        }

        public int CaseId { get; }
        public string CurrentUser { get;  }
        public List<CaseLogModel> CaseLogs { get; }
        public bool ShowInternalLogNotes { get; }
        public bool IsAttachmentsAllowed { get; set; }
        public CaseLogModel GetLastLog()
        {
            var log = CaseLogs.OrderByDescending(l => l.RegTime).FirstOrDefault();
            return log;
        }
    }

    public class CaseLogModel
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int? UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserSurName { get; set; }
        public string RegUserName { get; set; }
        public string InternalText { get; set; }
        public string ExternalText { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime RegTime { get; set; }

        public IList<LogFileModel> Files { get; set; }
    }

    public class LogFileModel
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public LogFileType LogType { get; set; }
    }
}
