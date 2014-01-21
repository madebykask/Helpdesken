using System;

namespace dhHelpdesk_NG.Domain
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeEMailLog : Entity
    {
        public int ChangeHistory_Id { get; set; }
        public int MailID { get; set; }
        public string EmailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ChangeEMailLogGUID { get; set; }

        public virtual ChangeEntity ChangeHistory { get; set; }
    }
}
