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

        public int CaseHistory_Id { get; set; }
        public int? Log_Id { get; set; }
        public int MailId { get; set; }
        public string EmailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid EmailLogGUID { get; set; }
        public DateTime? SendTime { get; set; }
        public string ResponseMessage { get; set; }

        public virtual CaseHistory CaseHistory { get; set; }

        public void SetResponse(DateTime? sendTime, string resopnseMessage)
        {
            this.SendTime = sendTime;
            this.ResponseMessage = resopnseMessage;
        }
    }
}
