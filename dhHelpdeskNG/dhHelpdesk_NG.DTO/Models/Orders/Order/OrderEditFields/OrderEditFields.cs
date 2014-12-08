namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderEditFields
    {
        public OrderEditFields(
                int? propertyId, 
                string orderRow1, 
                string orderRow2, 
                string orderRow3, 
                string orderRow4, 
                string orderRow5, 
                string orderRow6, 
                string orderRow7, 
                string orderRow8, 
                string configuration, 
                string orderInfo, 
                int orderInfo2)
        {
            this.OrderInfo2 = orderInfo2;
            this.OrderInfo = orderInfo;
            this.Configuration = configuration;
            this.OrderRow8 = orderRow8;
            this.OrderRow7 = orderRow7;
            this.OrderRow6 = orderRow6;
            this.OrderRow5 = orderRow5;
            this.OrderRow4 = orderRow4;
            this.OrderRow3 = orderRow3;
            this.OrderRow2 = orderRow2;
            this.OrderRow1 = orderRow1;
            this.PropertyId = propertyId;
        }

        [IsId]
        public int? PropertyId { get; private set; }
        
        [NotNull]
        public string OrderRow1 { get; private set; }

        [NotNull]
        public string OrderRow2 { get; private set; }

        [NotNull]
        public string OrderRow3 { get; private set; }

        [NotNull]
        public string OrderRow4 { get; private set; }

        [NotNull]
        public string OrderRow5 { get; private set; }

        [NotNull]
        public string OrderRow6 { get; private set; }

        [NotNull]
        public string OrderRow7 { get; private set; }

        [NotNull]
        public string OrderRow8 { get; private set; }

        [NotNull]
        public string Configuration { get; private set; }
        
        public string OrderInfo { get; private set; }

        public int OrderInfo2 { get; private set; }      
    }
}