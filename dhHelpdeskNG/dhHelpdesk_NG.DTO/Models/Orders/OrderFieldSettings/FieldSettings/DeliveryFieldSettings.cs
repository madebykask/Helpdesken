namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using Common.ValidationAttributes;

    public sealed class DeliveryFieldSettings : HeaderSettings
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
                TextFieldSettings deliveryOuId,
                TextFieldSettings name,
                TextFieldSettings phone)
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
            Name = name;
            Phone = phone;
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

        [NotNull]
        public TextFieldSettings Name { get; private set; }

        [NotNull]
        public TextFieldSettings Phone { get; private set; }

    }
}