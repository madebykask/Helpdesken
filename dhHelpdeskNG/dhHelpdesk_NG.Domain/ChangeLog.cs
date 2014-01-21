using System;

namespace dhHelpdesk_NG.Domain
{
    using dhHelpdesk_NG.Domain.Changes;

    public class ChangeLog : Entity
    {
        public int Change_Id { get; set; }
        public int ChangeEmailLog_Id { get; set; }
        public int ChangeHistory_Id { get; set; }
        public int ChangePart { get; set; }
        public string LogText { get; set; }
        public DateTime CreatedByUser_Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ChangeEntity Change { get; set; }
        public virtual ChangeEntity ChangeHistory { get; set; }
        public virtual EmailLog ChangeEmailLog { get; set; }
    }
}
