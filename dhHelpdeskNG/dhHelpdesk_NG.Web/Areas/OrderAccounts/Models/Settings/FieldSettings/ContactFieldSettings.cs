namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ContactFieldSettings
    {
        public ContactFieldSettings()
        {
        }

        public ContactFieldSettings(
            FieldSettingMultipleChoices ids,
            FieldSetting name,
            FieldSetting phone,
            FieldSetting email)
        {
            this.Ids = ids;
            this.Name = name;
            this.Phone = phone;
            this.Email = email;
        }

        [LocalizedDisplay("Id")]
        public FieldSettingMultipleChoices Ids { get;  set; }

        [LocalizedDisplay("Namn")]
        public FieldSetting Name { get;  set; }

        [LocalizedDisplay("Telefon")]
        public FieldSetting Phone { get;  set; }

        [LocalizedDisplay("E-post")]
        public FieldSetting Email { get;  set; }
    }
}