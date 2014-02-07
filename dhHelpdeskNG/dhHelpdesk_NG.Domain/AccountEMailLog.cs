namespace DH.Helpdesk.Domain
{
    using global::System;

    public class AccountEMailLog : Entity
    {
        public int Account_Id { get; set; }
        public int MailID { get; set; }
        public string EMailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid AccountEMailLogGUID { get; set; }

        public virtual Account Account { get; set; }
    }
}
