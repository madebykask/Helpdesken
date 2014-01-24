namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class OrdererFieldOverviewSettingGroupDto
    {
        public OrdererFieldOverviewSettingGroupDto(
            FieldOverviewSettingDto id,
            FieldOverviewSettingDto name,
            FieldOverviewSettingDto phone,
            FieldOverviewSettingDto cellPhone,
            FieldOverviewSettingDto email,
            FieldOverviewSettingDto department)
        {
            this.Id = id;
            this.Name = name;
            this.Phone = phone;
            this.CellPhone = cellPhone;
            this.Email = email;
            this.Department = department;
        }

        [NotNull]
        public FieldOverviewSettingDto Id { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Name { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Phone { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto CellPhone { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Email { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Department { get; private set; }
    }
}
