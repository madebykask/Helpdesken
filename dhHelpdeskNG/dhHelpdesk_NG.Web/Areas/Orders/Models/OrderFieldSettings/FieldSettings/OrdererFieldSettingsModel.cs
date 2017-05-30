using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class OrdererFieldSettingsModel
    {
        public OrdererFieldSettingsModel()
        {            
        }

        public OrdererFieldSettingsModel(
                MultiTextFieldSettingsModel ordererId, 
                TextFieldSettingsModel ordererName, 
                TextFieldSettingsModel ordererLocation, 
                TextFieldSettingsModel ordererEmail, 
                TextFieldSettingsModel ordererPhone, 
                TextFieldSettingsModel ordererCode, 
                TextFieldSettingsModel department, 
                TextFieldSettingsModel unit, 
                TextFieldSettingsModel ordererAddress, 
                TextFieldSettingsModel ordererInvoiceAddress, 
                TextFieldSettingsModel ordererReferenceNumber, 
                TextFieldSettingsModel accountingDimension1, 
                FieldSettingsModel accountingDimension2, 
                TextFieldSettingsModel accountingDimension3, 
                FieldSettingsModel accountingDimension4, 
                TextFieldSettingsModel accountingDimension5)
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

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererId)]
        public MultiTextFieldSettingsModel OrdererId { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererName)]
        public TextFieldSettingsModel OrdererName { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererLocation)]
        public TextFieldSettingsModel OrdererLocation { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererEmail)]
        public TextFieldSettingsModel OrdererEmail { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererPhone)]
        public TextFieldSettingsModel OrdererPhone { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererCode)]
        public TextFieldSettingsModel OrdererCode { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererDepartment)]
        public TextFieldSettingsModel Department { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererUnit)]
        public TextFieldSettingsModel Unit { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererAddress)]
        public TextFieldSettingsModel OrdererAddress { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererInvoiceAddress)]
        public TextFieldSettingsModel OrdererInvoiceAddress { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererReferenceNumber)]
        public TextFieldSettingsModel OrdererReferenceNumber { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererAccountingDimension1)]
        public TextFieldSettingsModel AccountingDimension1 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererAccountingDimension2)]
        public FieldSettingsModel AccountingDimension2 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererAccountingDimension3)]
        public TextFieldSettingsModel AccountingDimension3 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererAccountingDimension4)]
        public FieldSettingsModel AccountingDimension4 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrdererAccountingDimension5)]
        public TextFieldSettingsModel AccountingDimension5 { get; set; }     
    }
}