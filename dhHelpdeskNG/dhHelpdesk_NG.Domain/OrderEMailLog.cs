namespace DH.Helpdesk.Domain
{
    using DH.Helpdesk.Domain.Orders;

    using global::System;

    public class OrderEMailLog : Entity
    {

        public OrderEMailLog() {}

        public OrderEMailLog(int orderId, int orderHistoryId, int mailId, string email, string messageId)
        {
            this.Order_Id = orderId;
            this.OrderEMailLogGUID = Guid.NewGuid();
            this.OrderHistoryId = orderHistoryId;
            this.MailID = mailId;
            this.MessageId = messageId;
            this.EMailAddress = email;

        }

        public int MailID { get; set; }
        public int Order_Id { get; set; }
        public string EMailAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid OrderEMailLogGUID { get; set; }

        public int? OrderHistoryId { get; set; }

        public virtual Order Order { get; set; }

        public virtual OrderHistoryEntity OrderHistory { get; set; }
    }
}
