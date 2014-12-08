namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererEditFields
    {
        public OrdererEditFields(
                string ordererId, 
                string ordererName, 
                string ordererLocation, 
                string ordererEmail, 
                string ordererPhone, 
                string ordererCode, 
                int? departmentId, 
                int? unitId, 
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
            this.UnitId = unitId;
            this.DepartmentId = departmentId;
            this.OrdererCode = ordererCode;
            this.OrdererPhone = ordererPhone;
            this.OrdererEmail = ordererEmail;
            this.OrdererLocation = ordererLocation;
            this.OrdererName = ordererName;
            this.OrdererId = ordererId;
        }

        [NotNull]
        public string OrdererId { get; private set; }
        
        [NotNull]
        public string OrdererName { get; private set; }

        [NotNull]
        public string OrdererLocation { get; private set; }

        [NotNull]
        public string OrdererEmail { get; private set; }

        [NotNull]
        public string OrdererPhone { get; private set; }

        [NotNull]
        public string OrdererCode { get; private set; }
        
        [IsId]
        public int? DepartmentId { get; private set; }
        
        [IsId]
        public int? UnitId { get; private set; }
        
        [NotNull]
        public string OrdererAddress { get; private set; }
        
        [NotNull]
        public string OrdererInvoiceAddress { get; private set; }
        
        public string OrdererReferenceNumber { get; private set; }
        
        public string AccountingDimension1 { get; private set; }
        
        public string AccountingDimension2 { get; private set; }
        
        public string AccountingDimension3 { get; private set; }
        
        public string AccountingDimension4 { get; private set; }
        
        public string AccountingDimension5 { get; private set; }         
    }
}