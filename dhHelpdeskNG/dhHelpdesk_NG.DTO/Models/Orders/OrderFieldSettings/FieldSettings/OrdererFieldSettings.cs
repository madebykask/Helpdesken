namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererFieldSettings : HeaderSettings
    {
        public OrdererFieldSettings(
                MultiTextFieldSettings ordererId, 
                TextFieldSettings ordererName, 
                TextFieldSettings ordererLocation, 
                TextFieldSettings ordererEmail, 
                TextFieldSettings ordererPhone, 
                TextFieldSettings ordererCode, 
                TextFieldSettings department, 
                TextFieldSettings unit, 
                TextFieldSettings ordererAddress, 
                TextFieldSettings ordererInvoiceAddress, 
                TextFieldSettings ordererReferenceNumber, 
                TextFieldSettings accountingDimension1, 
                FieldSettings accountingDimension2, 
                TextFieldSettings accountingDimension3, 
                FieldSettings accountingDimension4, 
                TextFieldSettings accountingDimension5)
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
        public MultiTextFieldSettings OrdererId { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererName { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererLocation { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererEmail { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererPhone { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererCode { get; private set; }

        [NotNull]
        public TextFieldSettings Department { get; private set; }

        [NotNull]
        public TextFieldSettings Unit { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererAddress { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererInvoiceAddress { get; private set; }

        [NotNull]
        public TextFieldSettings OrdererReferenceNumber { get; private set; }

        [NotNull]
        public TextFieldSettings AccountingDimension1 { get; private set; }

        [NotNull]
        public FieldSettings AccountingDimension2 { get; private set; }

        [NotNull]
        public TextFieldSettings AccountingDimension3 { get; private set; }

        [NotNull]
        public FieldSettings AccountingDimension4 { get; private set; }

        [NotNull]
        public TextFieldSettings AccountingDimension5 { get; private set; }     
    }
}