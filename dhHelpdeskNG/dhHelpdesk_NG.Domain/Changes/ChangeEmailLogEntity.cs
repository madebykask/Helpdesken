namespace dhHelpdesk_NG.Domain.Changes
{
    using global::System;

    public class ChangeEmailLogEntity : Entity
    {
        public Guid ChangeEMailLogGUID { get; set; }

        public int ChangeHistory_Id { get; set; }

        public virtual ChangeEntity ChangeHistory { get; set; }

        public string EMailAddress { get; set; }

        public int MailID { get; set; }
        
        public string MessageId { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
