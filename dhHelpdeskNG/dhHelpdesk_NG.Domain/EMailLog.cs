using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    using global::System;

    public class EmailLog : Entity
    {
        public EmailLog() {}

        public EmailLog(int caseHistoryId, int mailId, string email, string messageId)
        {
            this.EmailLogGUID = Guid.NewGuid();
            this.CaseHistory_Id = caseHistoryId;
            this.MailId = mailId;
            this.MessageId = messageId;
            this.EmailAddress = email;
        }

        public int? CaseHistory_Id { get; set; }
        public int? Log_Id { get; set; }
        public int MailId { get; set; }
        public string EmailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid EmailLogGUID { get; set; }
        public DateTime? SendTime { get; set; }
        public string ResponseMessage { get; set; }

        public string Body { get; set; }
        public string Subject { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public bool HighPriority { get; set; }
        public string Files { get; set; }
        public string FilesInternal { get; set; }
        public string From { get; set; }
        public EmailSendStatus SendStatus { get; set; }
        public DateTime? LastAttempt { get; set; }
        public int? Attempts { get; set; }

        public virtual CaseHistory CaseHistory { get; set; }
        public virtual List<EmailLogAttempt> EmailLogAttempts { get; set; }
        
        public void SetResponse(DateTime? sendTime, string resopnseMessage)
        {
            SendTime = sendTime;
            ResponseMessage = resopnseMessage;
        }
    }
}
