namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderHistoryFields
{
    public sealed class OrderHistoryFields
    {
        public OrderHistoryFields(
                int? propertyId,
                string property, 
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
            this.PropertyId = propertyId;
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
            this.Property = property;
        }

        public int? PropertyId { get; private set; }

        public string Property { get; private set; }
        
        public string OrderRow1 { get; private set; }

        public string OrderRow2 { get; private set; }

        public string OrderRow3 { get; private set; }

        public string OrderRow4 { get; private set; }

        public string OrderRow5 { get; private set; }

        public string OrderRow6 { get; private set; }

        public string OrderRow7 { get; private set; }

        public string OrderRow8 { get; private set; }

        public string Configuration { get; private set; }
        
        public string OrderInfo { get; private set; }

        public int OrderInfo2 { get; private set; }      
    }
}