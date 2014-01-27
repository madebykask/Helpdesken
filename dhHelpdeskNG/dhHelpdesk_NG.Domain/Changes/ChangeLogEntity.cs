namespace dhHelpdesk_NG.Domain.Changes
{
    using global::System;

    public class ChangeLogEntity : Entity
    {
        public string LogText { get; set; }

        public int Change_Id { get; set; }

        public virtual ChangeEntity Change { get; set; }

        public int ChangePart { get; set; }

        public int? ChangeEMailLog_Id { get; set; }

        public virtual EmailLog ChangeEMailLog { get; set; }
        
        public int? ChangeHistory_Id { get; set; }

        public virtual ChangeEntity ChangeHistory { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? CreatedByUser_Id { get; set; }
    }
}
