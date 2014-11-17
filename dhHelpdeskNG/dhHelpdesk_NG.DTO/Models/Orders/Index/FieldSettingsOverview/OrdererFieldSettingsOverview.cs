namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererFieldSettingsOverview
    {
        public OrdererFieldSettingsOverview(
                FieldOverviewSetting ordererId, 
                FieldOverviewSetting ordererName, 
                FieldOverviewSetting ordererLocation, 
                FieldOverviewSetting ordererEmail, 
                FieldOverviewSetting ordererPhone, 
                FieldOverviewSetting ordererCode, 
                FieldOverviewSetting department, 
                FieldOverviewSetting unit, 
                FieldOverviewSetting ordererAddress, 
                FieldOverviewSetting ordererInvoiceAddress, 
                FieldOverviewSetting ordererReferenceNumber, 
                FieldOverviewSetting accountingDimension1, 
                FieldOverviewSetting accountingDimension2, 
                FieldOverviewSetting accountingDimension3, 
                FieldOverviewSetting accountingDimension4, 
                FieldOverviewSetting accountingDimension5)
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
        public FieldOverviewSetting OrdererId { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererName { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererLocation { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererEmail { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererPhone { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererCode { get; private set; }

        [NotNull]
        public FieldOverviewSetting Department { get; private set; }

        [NotNull]
        public FieldOverviewSetting Unit { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererAddress { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererInvoiceAddress { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrdererReferenceNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountingDimension1 { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountingDimension2 { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountingDimension3 { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountingDimension4 { get; private set; }

        [NotNull]
        public FieldOverviewSetting AccountingDimension5 { get; private set; }         
    }
}