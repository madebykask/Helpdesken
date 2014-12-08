namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReceiverEditFields
    {
        public ReceiverEditFields(
                string receiverId, 
                string receiverName, 
                string receiverEmail, 
                string receiverPhone, 
                string receiverLocation, 
                string markOfGoods)
        {
            this.MarkOfGoods = markOfGoods;
            this.ReceiverLocation = receiverLocation;
            this.ReceiverPhone = receiverPhone;
            this.ReceiverEmail = receiverEmail;
            this.ReceiverName = receiverName;
            this.ReceiverId = receiverId;
        }

        [NotNull]
        public string ReceiverId { get; private set; }

        [NotNull]
        public string ReceiverName { get; private set; }

        [NotNull]
        public string ReceiverEmail { get; private set; }

        [NotNull]
        public string ReceiverPhone { get; private set; }

        [NotNull]
        public string ReceiverLocation { get; private set; }

        [NotNull]
        public string MarkOfGoods { get; private set; }           
    }
}