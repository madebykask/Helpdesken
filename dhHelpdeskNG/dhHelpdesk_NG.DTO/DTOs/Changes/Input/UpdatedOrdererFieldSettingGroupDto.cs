namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class UpdatedOrdererFieldSettingGroupDto
    {
        public UpdatedOrdererFieldSettingGroupDto(
            UpdatedFieldSettingDto id,
            UpdatedFieldSettingDto name,
            UpdatedFieldSettingDto phone,
            UpdatedFieldSettingDto cellPhone,
            UpdatedFieldSettingDto email,
            UpdatedFieldSettingDto department)
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

        public UpdatedFieldSettingDto Id { get; private set; }

        public UpdatedFieldSettingDto Name { get; private set; }

        public UpdatedFieldSettingDto Phone { get; private set; }

        public UpdatedFieldSettingDto CellPhone { get; private set; }

        public UpdatedFieldSettingDto Email { get; private set; }

        public UpdatedFieldSettingDto Department { get; private set; }
    }
}
