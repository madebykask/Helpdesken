namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using Shared.Output;
    using Common.ValidationAttributes;

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
                FieldOverviewSetting deliveryOuId,
                FieldOverviewSetting deliveryName,
                FieldOverviewSetting deliveryPhone)
        {
            DeliveryOuId = deliveryOuId;
            DeliveryInfo3 = deliveryInfo3;
            DeliveryInfo2 = deliveryInfo2;
            DeliveryInfo1 = deliveryInfo1;
            DeliveryLocation = deliveryLocation;
            DeliveryPostalAddress = deliveryPostalAddress;
            DeliveryPostalCode = deliveryPostalCode;
            DeliveryAddress = deliveryAddress;
            DeliveryOu = deliveryOu;
            DeliveryDepartment = deliveryDepartment;
            InstallDate = installDate;
            DeliveryDate = deliveryDate;
            DeliveryName = deliveryName;
            DeliveryPhone = deliveryPhone;
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

        [NotNull]
        public FieldOverviewSetting DeliveryName { get; private set; }

        [NotNull]
        public FieldOverviewSetting DeliveryPhone { get; private set; }
    }
}