namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrdererEditSettings
    {
        public OrdererEditSettings(
            FieldEditSetting id,
            FieldEditSetting name,
            FieldEditSetting phone,
            FieldEditSetting cellPhone,
            FieldEditSetting email,
            FieldEditSetting department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public FieldEditSetting Id { get; private set; }

        [NotNull]
        public FieldEditSetting Name { get; private set; }

        [NotNull]
        public FieldEditSetting Phone { get; private set; }

        [NotNull]
        public FieldEditSetting CellPhone { get; private set; }

        [NotNull]
        public FieldEditSetting Email { get; private set; }

        [NotNull]
        public FieldEditSetting Department { get; private set; }
    }
}
