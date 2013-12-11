using System;

namespace dhHelpdesk_NG.Domain
{
    public class OrderEMailLog : Entity
    {
        public int MailID { get; set; }
        public int Order_Id { get; set; }
        public string EMailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid OrderEMailLogGUID { get; set; }

        public virtual Order Order { get; set; }
    }
}
