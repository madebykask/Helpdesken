namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class OrderedFieldSettingGroupDto
    {
        public OrderedFieldSettingGroupDto(
            FieldSettingDto name,
            FieldSettingDto phone,
            FieldSettingDto cellPhone,
            FieldSettingDto email,
            FieldSettingDto department)
        {
            ArgumentsValidator.NotNull(name, "name");
            ArgumentsValidator.NotNull(phone, "phone");
            ArgumentsValidator.NotNull(cellPhone, "cellPhone");
            ArgumentsValidator.NotNull(email, "email");
            ArgumentsValidator.NotNull(department, "department");

            Name = name;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Department = department;
        }

        public FieldSettingDto Name { get; private set; }

        public FieldSettingDto Phone { get; private set; }

        public FieldSettingDto CellPhone { get; private set; }

        public FieldSettingDto Email { get; private set; }

        public FieldSettingDto Department { get; private set; }
    }
}
