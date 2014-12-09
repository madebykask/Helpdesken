namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class OrdererFieldSettings
    {
        public OrdererFieldSettings(
            FieldSetting id,
            FieldSetting firstName,
            FieldSetting lastName,
            FieldSetting phone,
            FieldSetting email)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Email = email;
        }

        [LocalizedDisplay("Användar ID")]
        public FieldSetting Id { get; private set; }

        [LocalizedDisplay("Förnamn")]
        public FieldSetting FirstName { get; private set; }

        [LocalizedDisplay("Efternamn")]
        public FieldSetting LastName { get; private set; }

        [LocalizedDisplay("Telefon")]
        public FieldSetting Phone { get; private set; }

        [LocalizedDisplay("E-post")]
        public FieldSetting Email { get; private set; }
    }
}