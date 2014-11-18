namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DeliveryFieldSettingsOverview
    {
        public DeliveryFieldSettingsOverview(
                FieldOverviewSetting deliveryDate, 
                FieldOverviewSetting installDate, 
                FieldOverviewSetting deliveryDepartment, 
                FieldOverviewSetting deliveryOu, 
                FieldOverviewSetting deliveryAddress, 
                FieldOverviewSetting deliveryPostalCode, 
                FieldOverviewSetting deliveryPostalAddress, 
                FieldOverviewSetting deliveryLocation, 
                FieldOverviewSetting deliveryInfo1, 
                FieldOverviewSetting deliveryInfo2, 
                FieldOverviewSetting deliveryInfo3,
                FieldOverviewSetting deliveryOuId)
        {
            this.DeliveryOuId = deliveryOuId;
            this.DeliveryInfo3 = deliveryInfo3;
            this.DeliveryInfo2 = deliveryInfo2;
            this.DeliveryInfo1 = deliveryInfo1;
            this.DeliveryLocation = deliveryLocation;
            this.DeliveryPostalAddress = deliveryPostalAddress;
            this.DeliveryPostalCode = deliveryPostalCode;
            this.DeliveryAddress = deliveryAddress;
            this.DeliveryOu = deliveryOu;
            this.DeliveryDepartment = deliveryDepartment;
            this.InstallDate = installDate;
            this.DeliveryDate = deliveryDate;
        }

        [NotNull]
        public FieldOverviewSetting DeliveryDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting InstallDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryDepartment { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryOu { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryAddress { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryPostalCode { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryPostalAddress { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryLocation { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryInfo1 { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryInfo2 { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryInfo3 { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryOuId { get; private set; }
    }
}