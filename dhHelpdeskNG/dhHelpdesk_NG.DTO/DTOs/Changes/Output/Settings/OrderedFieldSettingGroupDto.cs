namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class OrderedFieldSettingGroupDto
    {
        public OrderedFieldSettingGroupDto(
            FieldSettingDto id,
            FieldSettingDto name,
            FieldSettingDto phone,
            FieldSettingDto cellPhone,
            FieldSettingDto email,
            FieldSettingDto department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public FieldSettingDto Id { get; private set; }

        [NotNull]
        public FieldSettingDto Name { get; private set; }

        [NotNull]
        public FieldSettingDto Phone { get; private set; }

        [NotNull]
        public FieldSettingDto CellPhone { get; private set; }

        [NotNull]
        public FieldSettingDto Email { get; private set; }

        [NotNull]
        public FieldSettingDto Department { get; private set; }
    }
}
