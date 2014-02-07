namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.SettingsEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererFieldSettings
    {
        public OrdererFieldSettings(
            FieldSetting id,
            FieldSetting name,
            FieldSetting phone,
            FieldSetting cellPhone,
            FieldSetting email,
            FieldSetting department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public FieldSetting Id { get; private set; }

        [NotNull]
        public FieldSetting Name { get; private set; }

        [NotNull]
        public FieldSetting Phone { get; private set; }

        [NotNull]
        public FieldSetting CellPhone { get; private set; }

        [NotNull]
        public FieldSetting Email { get; private set; }

        [NotNull]
        public FieldSetting Department { get; private set; }
    }
}
