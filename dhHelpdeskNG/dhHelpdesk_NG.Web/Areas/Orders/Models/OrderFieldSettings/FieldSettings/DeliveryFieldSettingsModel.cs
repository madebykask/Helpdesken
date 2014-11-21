namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class DeliveryFieldSettingsModel
    {
        public DeliveryFieldSettingsModel()
        {            
        }

        public DeliveryFieldSettingsModel(
                TextFieldSettingsModel deliveryDate, 
                TextFieldSettingsModel installDate, 
                TextFieldSettingsModel deliveryDepartment, 
                TextFieldSettingsModel deliveryOu, 
                TextFieldSettingsModel deliveryAddress, 
                TextFieldSettingsModel deliveryPostalCode, 
                TextFieldSettingsModel deliveryPostalAddress, 
                TextFieldSettingsModel deliveryLocation, 
                TextFieldSettingsModel deliveryInfo1, 
                TextFieldSettingsModel deliveryInfo2, 
                TextFieldSettingsModel deliveryInfo3, 
                TextFieldSettingsModel deliveryOuId)
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
        [LocalizedDisplay("Leveransdatum")]
        public TextFieldSettingsModel DeliveryDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Installationsdatum")]
        public TextFieldSettingsModel InstallDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Avdelning")]
        public TextFieldSettingsModel DeliveryDepartment { get; set; }

        [NotNull]
        [LocalizedDisplay("Enhet")]
        public TextFieldSettingsModel DeliveryOu { get; set; }

        [NotNull]
        [LocalizedDisplay("adress")]
        public TextFieldSettingsModel DeliveryAddress { get; set; }

        [NotNull]
        [LocalizedDisplay("Postnummer")]
        public TextFieldSettingsModel DeliveryPostalCode { get; set; }

        [NotNull]
        [LocalizedDisplay("Postadress")]
        public TextFieldSettingsModel DeliveryPostalAddress { get; set; }

        [NotNull]
        [LocalizedDisplay("Placering")]
        public TextFieldSettingsModel DeliveryLocation { get; set; }

        [NotNull]
        [LocalizedDisplay("Info")]
        public TextFieldSettingsModel DeliveryInfo1 { get; set; }

        [NotNull]
        [LocalizedDisplay("Info")]
        public TextFieldSettingsModel DeliveryInfo2 { get; set; }

        [NotNull]
        [LocalizedDisplay("Info")]
        public TextFieldSettingsModel DeliveryInfo3 { get; set; }

        [NotNull]
        [LocalizedDisplay("Enhet")]
        public TextFieldSettingsModel DeliveryOuId { get; set; }
    }
}