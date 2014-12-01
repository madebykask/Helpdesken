namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReceiverEditSettings
    {
         public ReceiverEditSettings(
                TextFieldEditSettings receiverId, 
                TextFieldEditSettings receiverName, 
                TextFieldEditSettings receiverEmail, 
                TextFieldEditSettings receiverPhone, 
                TextFieldEditSettings receiverLocation, 
                TextFieldEditSettings markOfGoods)
        {
            this.MarkOfGoods = markOfGoods;
            this.ReceiverLocation = receiverLocation;
            this.ReceiverPhone = receiverPhone;
            this.ReceiverEmail = receiverEmail;
            this.ReceiverName = receiverName;
            this.ReceiverId = receiverId;
        }

        [NotNull]
        public TextFieldEditSettings ReceiverId { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings ReceiverName { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings ReceiverEmail { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings ReceiverPhone { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings ReceiverLocation { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings MarkOfGoods { get; private set; }  
    }
}