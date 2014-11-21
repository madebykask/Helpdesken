namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReceiverFieldSettings
    {
        public ReceiverFieldSettings(
                TextFieldSettings receiverId, 
                TextFieldSettings receiverName, 
                TextFieldSettings receiverEmail, 
                TextFieldSettings receiverPhone, 
                TextFieldSettings receiverLocation, 
                TextFieldSettings markOfGoods)
        {
            this.MarkOfGoods = markOfGoods;
            this.ReceiverLocation = receiverLocation;
            this.ReceiverPhone = receiverPhone;
            this.ReceiverEmail = receiverEmail;
            this.ReceiverName = receiverName;
            this.ReceiverId = receiverId;
        }

        [NotNull]
        public TextFieldSettings ReceiverId { get; private set; }
         
        [NotNull]
        public TextFieldSettings ReceiverName { get; private set; }
         
        [NotNull]
        public TextFieldSettings ReceiverEmail { get; private set; }
         
        [NotNull]
        public TextFieldSettings ReceiverPhone { get; private set; }
         
        [NotNull]
        public TextFieldSettings ReceiverLocation { get; private set; }
         
        [NotNull]
        public TextFieldSettings MarkOfGoods { get; private set; }                  
    }
}