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
                TextFieldSettingsModel ordererId, 
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

        [NotNull]
        [LocalizedDisplay("Identifierare")]
        public TextFieldSettingsModel OrdererId { get; set; }

        [NotNull]
        [LocalizedDisplay("Namn")]
        public TextFieldSettingsModel OrdererName { get; set; }

        [NotNull]
        [LocalizedDisplay("Placering")]
        public TextFieldSettingsModel OrdererLocation { get; set; }

        [NotNull]
        [LocalizedDisplay("E-post")]
        public TextFieldSettingsModel OrdererEmail { get; set; }

        [NotNull]
        [LocalizedDisplay("Telefon")]
        public TextFieldSettingsModel OrdererPhone { get; set; }

        [NotNull]
        [LocalizedDisplay("Ansvarskod")]
        public TextFieldSettingsModel OrdererCode { get; set; }

        [NotNull]
        [LocalizedDisplay("Avdelning")]
        public TextFieldSettingsModel Department { get; set; }

        [NotNull]
        [LocalizedDisplay("Enhet")]
        public TextFieldSettingsModel Unit { get; set; }

        [NotNull]
        [LocalizedDisplay("adress")]
        public TextFieldSettingsModel OrdererAddress { get; set; }

        [NotNull]
        [LocalizedDisplay("Fakturaadress")]
        public TextFieldSettingsModel OrdererInvoiceAddress { get; set; }

        [NotNull]
        [LocalizedDisplay("Referensnummer")]
        public TextFieldSettingsModel OrdererReferenceNumber { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension")]
        public TextFieldSettingsModel AccountingDimension1 { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension")]
        public FieldSettingsModel AccountingDimension2 { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension")]
        public TextFieldSettingsModel AccountingDimension3 { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension")]
        public FieldSettingsModel AccountingDimension4 { get; set; }

        [NotNull]
        [LocalizedDisplay("Kontodimension")]
        public TextFieldSettingsModel AccountingDimension5 { get; set; }     
    }
}