using System;

namespace dhHelpdesk_NG.Domain
{
    public class OperationLogEMailLog : Entity
    {
        public int OperationLog_Id { get; set; }
        public string SMSText { get; set; }
        public string Recipients { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual OperationLog OperationLog { get; set; }
    }
}
