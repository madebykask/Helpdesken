namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
            Id = id;
            Name = name;
            Phone = phone;
            CellPhone = cellPhone;
            Email = email;
            Department = department;
        }

        [NotNull]
        public UpdatedFieldSettingDto Id { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Name { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Phone { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto CellPhone { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Email { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Department { get; private set; }
    }
}
