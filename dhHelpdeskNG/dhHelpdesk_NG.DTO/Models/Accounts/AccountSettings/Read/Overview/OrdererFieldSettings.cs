namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview
{
    using DH.Helpdesk.Common.ValidationAttributes;

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

        [IsId]
        public FieldSetting Id { get; private set; }

        public FieldSetting FirstName { get; private set; }

        public FieldSetting LastName { get; private set; }

        public FieldSetting Phone { get; private set; }

        public FieldSetting Email { get; private set; }
    }
}