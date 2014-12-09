namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class DeliveryInformationFieldSettings
    {
        public DeliveryInformationFieldSettings()
        {
        }

        public DeliveryInformationFieldSettings(
            FieldSetting name,
            FieldSetting phone,
            FieldSetting address,
            FieldSetting postalAddress)
        {
            this.Name = name;
            this.Phone = phone;
            this.Address = address;
            this.PostalAddress = postalAddress;
        }

        [LocalizedDisplay("Namn")]
        public FieldSetting Name { get;  set; }

        [LocalizedDisplay("Telefon")]
        public FieldSetting Phone { get;  set; }

        [LocalizedDisplay("Adress")]
        public FieldSetting Address { get;  set; }

        [LocalizedDisplay("E-Postadress")]
        public FieldSetting PostalAddress { get;  set; }
    }
}