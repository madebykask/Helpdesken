namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DeliveryFieldSettings
    {
        public DeliveryFieldSettings(
                TextFieldSettings deliveryDate, 
                TextFieldSettings installDate, 
                TextFieldSettings deliveryDepartment, 
                TextFieldSettings deliveryOu, 
                TextFieldSettings deliveryAddress, 
                TextFieldSettings deliveryPostalCode, 
                TextFieldSettings deliveryPostalAddress, 
                TextFieldSettings deliveryLocation, 
                TextFieldSettings deliveryInfo1, 
                TextFieldSettings deliveryInfo2, 
                TextFieldSettings deliveryInfo3, 
                TextFieldSettings deliveryOuId)
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
        public TextFieldSettings DeliveryDate { get; private set; }

        [NotNull]
        public TextFieldSettings InstallDate { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryDepartment { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryOu { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryAddress { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryPostalCode { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryPostalAddress { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryLocation { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryInfo1 { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryInfo2 { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryInfo3 { get; private set; }

        [NotNull]
        public TextFieldSettings DeliveryOuId { get; private set; }
    }
}