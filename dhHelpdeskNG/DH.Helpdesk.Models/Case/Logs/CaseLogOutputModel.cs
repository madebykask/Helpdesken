using System;
using System.Collections.Generic;

namespace DH.Helpdesk.Models.Case.Logs
{
    public class CaseLogOutputModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsExternal { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public IList<EmailLogModel> EmailLogs { get; set; }
        public IList<LogFileModel> Files { get; set; }
        public IList<Mail2TicketModel> Mail2Tickets { get; set; }
    }

    public class LogFileModel
    {
        public int Id { get; set; }
        public int LogId { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CaseId { get; set; }
    }

    public class EmailLogModel
    {
        public int Id { get; set; }
        public int MailId { get; set; }
        public string Email { get; set; }
    }

    public class Mail2TicketModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}
