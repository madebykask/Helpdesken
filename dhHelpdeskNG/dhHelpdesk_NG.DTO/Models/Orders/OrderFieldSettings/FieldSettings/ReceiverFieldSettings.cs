namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReceiverFieldSettings
    {
        public ReceiverFieldSettings(
                FieldSettings receiverId, 
                FieldSettings receiverName, 
                FieldSettings receiverEmail, 
                FieldSettings receiverPhone, 
                FieldSettings receiverLocation, 
                FieldSettings markOfGoods)
        {
            this.MarkOfGoods = markOfGoods;
            this.ReceiverLocation = receiverLocation;
            this.ReceiverPhone = receiverPhone;
            this.ReceiverEmail = receiverEmail;
            this.ReceiverName = receiverName;
            this.ReceiverId = receiverId;
        }

        [NotNull]
        public FieldSettings ReceiverId { get; private set; }
         
        [NotNull]
        public FieldSettings ReceiverName { get; private set; }
         
        [NotNull]
        public FieldSettings ReceiverEmail { get; private set; }
         
        [NotNull]
        public FieldSettings ReceiverPhone { get; private set; }
         
        [NotNull]
        public FieldSettings ReceiverLocation { get; private set; }
         
        [NotNull]
        public FieldSettings MarkOfGoods { get; private set; }                  
    }
}