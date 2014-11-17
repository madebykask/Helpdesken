namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write
{
    public sealed class ContactFieldSettings
    {
        public ContactFieldSettings(FieldSettingMultipleChoices ids, FieldSetting name, FieldSetting phone, FieldSetting email)
        {
            this.Ids = ids;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        public FieldSettingMultipleChoices Ids { get; private set; }

        public FieldSetting Name { get; private set; }

        public FieldSetting Phone { get; private set; }

        public FieldSetting Email { get; private set; }
    }
}