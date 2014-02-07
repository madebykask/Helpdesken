namespace DH.Helpdesk.Domain
{
    using global::System;

    public class EmailLog : Entity
    {
        public int CaseHistory_Id { get; set; }
        public int? Log_Id { get; set; }
        public int MailId { get; set; }
        public string EmailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid EmailLogGUID { get; set; }

        public virtual CaseHistory CaseHistory { get; set; }
        //public virtual Log Log { get; set; }
    }
}
