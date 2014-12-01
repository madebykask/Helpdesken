namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
                TextFieldEditSettings deliveryOuId)
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
    }
}