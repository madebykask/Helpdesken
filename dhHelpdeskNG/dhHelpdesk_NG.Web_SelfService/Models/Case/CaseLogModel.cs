using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.SelfService.Models.Case
{
    public class CaseLogModel 
    {
        public CaseLogModel()
        { 
        }

        public CaseLogModel(int caseId, List<Log> caseLogs, string currentUser, bool showInteralLog = false) 
        {
            CaseId = caseId;
            CurrentUser = currentUser;
            CaseLogs = caseLogs;
            ShowInternalLogNotes = showInteralLog;
        }

        public int CaseId { get; }
        public string CurrentUser { get;  }
        public List<Log> CaseLogs { get; }
        public bool ShowInternalLogNotes { get; }

        public List<Log> GetCaseLogsForExternalPage()
        {
            var filter =
                ShowInternalLogNotes
                    ? (Func<Log, bool>)(l => !string.IsNullOrEmpty(l.Text_Internal.Trim()))
                    : (Func<Log, bool>)(l => !string.IsNullOrEmpty(l.Text_External.Trim()));

            var logs = CaseLogs?.Where(filter).OrderByDescending(l => l.RegTime).ToList();
            return logs;
        }

        public Log GetLastLogForExternalPage()
        {
            var log = GetCaseLogsForExternalPage().FirstOrDefault();
            return log;
        }
    }
}
