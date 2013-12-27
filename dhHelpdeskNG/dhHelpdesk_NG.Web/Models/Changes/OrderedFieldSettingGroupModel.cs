namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class OrderedFieldSettingGroupModel
    {
        public OrderedFieldSettingGroupModel()
        {
        }

        public OrderedFieldSettingGroupModel(
            FieldSettingModel id,
            FieldSettingModel name,
            FieldSettingModel phone,
            FieldSettingModel cellPhone,
            FieldSettingModel email,
            FieldSettingModel department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public FieldSettingModel Id { get; private set; }

        [NotNull]
        public FieldSettingModel Name { get; private set; }

        [NotNull]
        public FieldSettingModel Phone { get; private set; }

        [NotNull]
        public FieldSettingModel CellPhone { get; private set; }

        [NotNull]
        public FieldSettingModel Email { get; private set; }

        [NotNull]
        public FieldSettingModel Department { get; private set; }
    }
}
