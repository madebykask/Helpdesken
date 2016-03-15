namespace DH.Helpdesk.Domain
{
    using global::System;

    public class OperationLogEMailLog : Entity
    {

        public OperationLogEMailLog() {}

        public OperationLogEMailLog(int operationLogId, string smstext, string recipients)
        {
            this.OperationLog_Id = operationLogId;
            this.SMSText = smstext;
            this.Recipients = recipients;
        }

        public int? OperationLog_Id { get; set; }
        public string SMSText { get; set; }
        public string Recipients { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual OperationLog OperationLog { get; set; }
    }
}
