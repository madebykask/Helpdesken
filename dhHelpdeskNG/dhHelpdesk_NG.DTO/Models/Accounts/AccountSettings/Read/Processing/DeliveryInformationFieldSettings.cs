namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing
{
    public sealed class DeliveryInformationFieldSettings
    {
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

        public FieldSetting Name { get; private set; }

        public FieldSetting Phone { get; private set; }

        public FieldSetting Address { get; private set; }

        public FieldSetting PostalAddress { get; private set; }
    }
}