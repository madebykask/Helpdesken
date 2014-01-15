namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class GeneralFieldOverviewSettingGroupDto
    {
        public GeneralFieldOverviewSettingGroupDto(
            FieldOverviewSettingDto priority,
            FieldOverviewSettingDto title,
            FieldOverviewSettingDto state,
            FieldOverviewSettingDto system,
            FieldOverviewSettingDto @object,
            FieldOverviewSettingDto inventory,
            FieldOverviewSettingDto owner,
            FieldOverviewSettingDto workingGroup,
            FieldOverviewSettingDto administrator,
            FieldOverviewSettingDto finishingDate,
            FieldOverviewSettingDto rss)
        {
            Priority = priority;
            Title = title;
            State = state;
            System = system;
            Object = @object;
            Inventory = inventory;
            Owner = owner;
            WorkingGroup = workingGroup;
            Administrator = administrator;
            FinishingDate = finishingDate;
            Rss = rss;
        }

        [NotNull]
        public FieldOverviewSettingDto Priority { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Title { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto State { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto System { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Object { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Inventory { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Owner { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto WorkingGroup { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Administrator { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto FinishingDate { get; private set; }

        [NotNull]
        public FieldOverviewSettingDto Rss { get; private set; }
    }
}
