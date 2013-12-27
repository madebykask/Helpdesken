namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class UpdatedGeneralFieldSettingGroupDto
    {
        public UpdatedGeneralFieldSettingGroupDto(
            UpdatedFieldSettingDto priority,
            UpdatedFieldSettingDto title,
            UpdatedFieldSettingDto state,
            UpdatedFieldSettingDto system,
            UpdatedFieldSettingDto @object,
            UpdatedFieldSettingDto inventory,
            UpdatedFieldSettingDto owner,
            UpdatedFieldSettingDto workingGroup,
            UpdatedFieldSettingDto administrator,
            UpdatedFieldSettingDto finishingDate,
            UpdatedFieldSettingDto rss)
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
        public UpdatedFieldSettingDto Priority { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Title { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto State { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto System { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Object { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Inventory { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Owner { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto WorkingGroup { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Administrator { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto FinishingDate { get; private set; }

        [NotNull]
        public UpdatedFieldSettingDto Rss { get; private set; }
    }
}
