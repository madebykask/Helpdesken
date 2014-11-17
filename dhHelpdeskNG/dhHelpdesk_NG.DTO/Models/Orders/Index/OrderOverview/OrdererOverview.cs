namespace DH.Helpdesk.BusinessData.Models.Orders.Index.OrderOverview
{
    public sealed class OrdererOverview
    {
        public OrdererOverview(
                string ordererId, 
                string ordererName, 
                string ordererLocation, 
                string ordererEmail, 
                string ordererPhone, 
                string ordererCode, 
                string department, 
                string unit, 
                string ordererAddress, 
                string ordererInvoiceAddress, 
                string ordererReferenceNumber, 
                string accountingDimension1, 
                string accountingDimension2, 
                string accountingDimension3, 
                string accountingDimension4, 
                string accountingDimension5)
        {
            this.AccountingDimension5 = accountingDimension5;
            this.AccountingDimension4 = accountingDimension4;
            this.AccountingDimension3 = accountingDimension3;
            this.AccountingDimension2 = accountingDimension2;
            this.AccountingDimension1 = accountingDimension1;
            this.OrdererReferenceNumber = ordererReferenceNumber;
            this.OrdererInvoiceAddress = ordererInvoiceAddress;
            this.OrdererAddress = ordererAddress;
            this.Unit = unit;
            this.Department = department;
            this.OrdererCode = ordererCode;
            this.OrdererPhone = ordererPhone;
            this.OrdererEmail = ordererEmail;
            this.OrdererLocation = ordererLocation;
            this.OrdererName = ordererName;
            this.OrdererId = ordererId;
        }

        public string OrdererId { get; private set; }
        
        public string OrdererName { get; private set; }
        
        public string OrdererLocation { get; private set; }
        
        public string OrdererEmail { get; private set; }
        
        public string OrdererPhone { get; private set; }
        
        public string OrdererCode { get; private set; }
        
        public string Department { get; private set; }
        
        public string Unit { get; private set; }
        
        public string OrdererAddress { get; private set; }
        
        public string OrdererInvoiceAddress { get; private set; }
        
        public string OrdererReferenceNumber { get; private set; }
        
        public string AccountingDimension1 { get; private set; }
        
        public string AccountingDimension2 { get; private set; }
        
        public string AccountingDimension3 { get; private set; }
        
        public string AccountingDimension4 { get; private set; }
        
        public string AccountingDimension5 { get; private set; }         
    }
}