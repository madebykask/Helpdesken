namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using Common.ValidationAttributes;

    public sealed class DeliveryEditSettings
    {
         public DeliveryEditSettings(
                TextFieldEditSettings deliveryDate, 
                TextFieldEditSettings installDate, 
                TextFieldEditSettings deliveryDepartment, 
                TextFieldEditSettings deliveryOu, 
                TextFieldEditSettings deliveryAddress, 
                TextFieldEditSettings deliveryPostalCode, 
                TextFieldEditSettings deliveryPostalAddress, 
                TextFieldEditSettings deliveryLocation, 
                TextFieldEditSettings deliveryInfo1, 
                TextFieldEditSettings deliveryInfo2, 
                TextFieldEditSettings deliveryInfo3, 
                TextFieldEditSettings deliveryOuId,
                TextFieldEditSettings deliveryName,
                TextFieldEditSettings deliveryPhone)
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
        public TextFieldEditSettings DeliveryDate { get; private set; }

        [NotNull]
        public TextFieldEditSettings InstallDate { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryDepartment { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryOu { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryAddress { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryPostalCode { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryPostalAddress { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryLocation { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryInfo1 { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryInfo2 { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryInfo3 { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryOuId { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryName { get; private set; }

        [NotNull]
        public TextFieldEditSettings DeliveryPhone { get; private set; }


    }
}