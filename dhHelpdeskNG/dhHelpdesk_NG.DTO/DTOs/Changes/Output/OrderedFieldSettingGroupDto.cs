namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

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
            ArgumentsValidator.NotNull(id, "id");
            ArgumentsValidator.NotNull(name, "name");
            ArgumentsValidator.NotNull(phone, "phone");
            ArgumentsValidator.NotNull(cellPhone, "cellPhone");
            ArgumentsValidator.NotNull(email, "email");
            ArgumentsValidator.NotNull(department, "department");

            Id = id;
            Name = name;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Department = department;
        }

        public FieldSettingDto Id { get; private set; }

        public FieldSettingDto Name { get; private set; }

        public FieldSettingDto Phone { get; private set; }

        public FieldSettingDto CellPhone { get; private set; }

        public FieldSettingDto Email { get; private set; }

        public FieldSettingDto Department { get; private set; }
    }
}
