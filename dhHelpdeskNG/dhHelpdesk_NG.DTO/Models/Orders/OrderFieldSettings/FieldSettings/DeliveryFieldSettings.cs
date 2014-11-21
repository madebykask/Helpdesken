namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DeliveryFieldSettings
    {
        public DeliveryFieldSettings(
                FieldSettings deliveryDate, 
                FieldSettings installDate, 
                FieldSettings deliveryDepartment, 
                FieldSettings deliveryOu, 
                FieldSettings deliveryAddress, 
                FieldSettings deliveryPostalCode, 
                FieldSettings deliveryPostalAddress, 
                FieldSettings deliveryLocation, 
                FieldSettings deliveryInfo1, 
                FieldSettings deliveryInfo2, 
                FieldSettings deliveryInfo3, 
                FieldSettings deliveryOuId)
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
        public FieldSettings DeliveryDate { get; private set; }

        [NotNull]
        public FieldSettings InstallDate { get; private set; }

        [NotNull]
        public FieldSettings DeliveryDepartment { get; private set; }

        [NotNull]
        public FieldSettings DeliveryOu { get; private set; }

        [NotNull]
        public FieldSettings DeliveryAddress { get; private set; }

        [NotNull]
        public FieldSettings DeliveryPostalCode { get; private set; }

        [NotNull]
        public FieldSettings DeliveryPostalAddress { get; private set; }

        [NotNull]
        public FieldSettings DeliveryLocation { get; private set; }

        [NotNull]
        public FieldSettings DeliveryInfo1 { get; private set; }

        [NotNull]
        public FieldSettings DeliveryInfo2 { get; private set; }

        [NotNull]
        public FieldSettings DeliveryInfo3 { get; private set; }

        [NotNull]
        public FieldSettings DeliveryOuId { get; private set; }
    }
}