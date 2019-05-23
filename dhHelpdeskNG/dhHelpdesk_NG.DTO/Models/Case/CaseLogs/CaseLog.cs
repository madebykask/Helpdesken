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
        public IList<Mail2TicketData> Mail2Tickets { get; set; }
    }

    public class LogFileData
    {
        public LogFileData()
        {
        }

        public LogFileData(int id, string fileName, int logId, int? caseId)
        {
            Id = id;
            FileName = fileName;
            LogId = logId;
            CaseId = caseId;
        }

        public int Id { get; set; }
        public string FileName { get; set; }
        public int LogId { get; set; }
        public int? CaseId { get; set; }
    }

    public class Mail2TicketData
    {
        public Mail2TicketData()
        {
        }

        public Mail2TicketData(int id, string type, string email, string subject)
        {
            Id = id;
            Type = type;
            Email = email;
            Subject = subject;
        }

        public int Id { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}