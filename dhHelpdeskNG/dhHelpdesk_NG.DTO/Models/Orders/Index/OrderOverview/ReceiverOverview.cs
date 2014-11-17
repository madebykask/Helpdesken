namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class ReceiverOverview
    {
        public ReceiverOverview(
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

        public string ReceiverId { get; private set; }
        
        public string ReceiverName { get; private set; }
        
        public string ReceiverEmail { get; private set; }
        
        public string ReceiverPhone { get; private set; }
        
        public string ReceiverLocation { get; private set; }
        
        public string MarkOfGoods { get; private set; }   
    }
}