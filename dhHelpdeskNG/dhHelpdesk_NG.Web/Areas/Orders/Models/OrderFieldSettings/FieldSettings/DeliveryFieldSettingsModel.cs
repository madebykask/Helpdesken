namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using Common.ValidationAttributes;
    using Web.Infrastructure.LocalizedAttributes;

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
                TextFieldSettingsModel deliveryOuId,
                TextFieldSettingsModel name,
                TextFieldSettingsModel phone)
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

        public string Header { get; set; }

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

        [NotNull]
        [LocalizedDisplay("Namn")]
        public TextFieldSettingsModel Name { get; set; }


        [NotNull]
        [LocalizedDisplay("Telefon")]
        public TextFieldSettingsModel Phone { get; set; }
    }
}