namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererFieldSettings
    {
        public OrdererFieldSettings(
                FieldSettings ordererId, 
                FieldSettings ordererName, 
                FieldSettings ordererLocation, 
                FieldSettings ordererEmail, 
                FieldSettings ordererPhone, 
                FieldSettings ordererCode, 
                FieldSettings department, 
                FieldSettings unit, 
                FieldSettings ordererAddress, 
                FieldSettings ordererInvoiceAddress, 
                FieldSettings ordererReferenceNumber, 
                FieldSettings accountingDimension1, 
                TextFieldSettings accountingDimension2, 
                FieldSettings accountingDimension3, 
                TextFieldSettings accountingDimension4, 
                FieldSettings accountingDimension5)
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

        [NotNull]
        public FieldSettings OrdererId { get; private set; }

        [NotNull]
        public FieldSettings OrdererName { get; private set; }

        [NotNull]
        public FieldSettings OrdererLocation { get; private set; }

        [NotNull]
        public FieldSettings OrdererEmail { get; private set; }

        [NotNull]
        public FieldSettings OrdererPhone { get; private set; }

        [NotNull]
        public FieldSettings OrdererCode { get; private set; }

        [NotNull]
        public FieldSettings Department { get; private set; }

        [NotNull]
        public FieldSettings Unit { get; private set; }

        [NotNull]
        public FieldSettings OrdererAddress { get; private set; }

        [NotNull]
        public FieldSettings OrdererInvoiceAddress { get; private set; }

        [NotNull]
        public FieldSettings OrdererReferenceNumber { get; private set; }

        [NotNull]
        public FieldSettings AccountingDimension1 { get; private set; }

        [NotNull]
        public TextFieldSettings AccountingDimension2 { get; private set; }

        [NotNull]
        public FieldSettings AccountingDimension3 { get; private set; }

        [NotNull]
        public TextFieldSettings AccountingDimension4 { get; private set; }

        [NotNull]
        public FieldSettings AccountingDimension5 { get; private set; }     
    }
}