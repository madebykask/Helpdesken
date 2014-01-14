namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class GeneralFieldSettingGroupDto
    {
        public GeneralFieldSettingGroupDto(
            FieldSettingDto priority,
            FieldSettingDto title,
            FieldSettingDto state,
            FieldSettingDto system,
            FieldSettingDto @object,
            FieldSettingDto inventory,
            FieldSettingDto owner,
            FieldSettingDto workingGroup,
            FieldSettingDto administrator,
            FieldSettingDto finishingDate,
            FieldSettingDto rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.State = state;
            this.System = system;
            this.Object = @object;
            this.Inventory = inventory;
            this.Owner = owner;
            this.WorkingGroup = workingGroup;
            this.Administrator = administrator;
            this.FinishingDate = finishingDate;
            this.Rss = rss;
        }

        [NotNull]
        public FieldSettingDto Priority { get; private set; }

        [NotNull]
        public FieldSettingDto Title { get; private set; }

        [NotNull]
        public FieldSettingDto State { get; private set; }

        [NotNull]
        public FieldSettingDto System { get; private set; }

        [NotNull]
        public FieldSettingDto Object { get; private set; }

        [NotNull]
        public FieldSettingDto Inventory { get; private set; }

        [NotNull]
        public FieldSettingDto Owner { get; private set; }

        [NotNull]
        public FieldSettingDto WorkingGroup { get; private set; }

        [NotNull]
        public FieldSettingDto Administrator { get; private set; }

        [NotNull]
        public FieldSettingDto FinishingDate { get; private set; }

        [NotNull]
        public FieldSettingDto Rss { get; private set; }
    }
}
