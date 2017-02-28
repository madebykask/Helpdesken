using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererEditSettings : HeaderSettings
    {
        public OrdererEditSettings(
                MultiTextFieldEditSettings ordererId, 
                TextFieldEditSettings ordererName, 
                TextFieldEditSettings ordererLocation, 
                TextFieldEditSettings ordererEmail, 
                TextFieldEditSettings ordererPhone, 
                TextFieldEditSettings ordererCode, 
                TextFieldEditSettings department, 
                TextFieldEditSettings unit, 
                TextFieldEditSettings ordererAddress, 
                TextFieldEditSettings ordererInvoiceAddress, 
                TextFieldEditSettings ordererReferenceNumber, 
                TextFieldEditSettings accountingDimension1, 
                FieldEditSettings accountingDimension2, 
                TextFieldEditSettings accountingDimension3, 
                FieldEditSettings accountingDimension4, 
                TextFieldEditSettings accountingDimension5)
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
        public MultiTextFieldEditSettings OrdererId { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererName { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererLocation { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererEmail { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererPhone { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererCode { get; private set; }

        [NotNull]
        public TextFieldEditSettings Department { get; private set; }

        [NotNull]
        public TextFieldEditSettings Unit { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererAddress { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererInvoiceAddress { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrdererReferenceNumber { get; private set; }

        [NotNull]
        public TextFieldEditSettings AccountingDimension1 { get; private set; }

        [NotNull]
        public FieldEditSettings AccountingDimension2 { get; private set; }

        [NotNull]
        public TextFieldEditSettings AccountingDimension3 { get; private set; }

        [NotNull]
        public FieldEditSettings AccountingDimension4 { get; private set; }

        [NotNull]
        public TextFieldEditSettings AccountingDimension5 { get; private set; }     
    }
}