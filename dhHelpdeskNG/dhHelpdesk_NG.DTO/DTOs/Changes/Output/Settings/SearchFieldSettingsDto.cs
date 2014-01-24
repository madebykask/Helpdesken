namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class SearchFieldSettingsDto
    {
        public SearchFieldSettingsDto(
            FieldOverviewSettingDto statuses,
            FieldOverviewSettingDto objects,
            FieldOverviewSettingDto workingGroups,
            FieldOverviewSettingDto administrators)
        {
            this.Statuses = statuses;
            this.Objects = @objects;
            this.WorkingGroups = workingGroups;
            this.Administrators = administrators;
        }

        [NotNull]
        public FieldOverviewSettingDto Statuses { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Objects { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto WorkingGroups { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Administrators { get; private set; }
    }
}
