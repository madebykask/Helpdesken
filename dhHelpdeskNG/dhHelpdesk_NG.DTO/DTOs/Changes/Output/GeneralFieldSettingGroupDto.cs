namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class GeneralFieldSettingGroupDto
    {
        public GeneralFieldSettingGroupDto(FieldSettingDto priority,
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
            ArgumentsValidator.NotNull(priority, "priority");
            ArgumentsValidator.NotNull(title, "title");
            ArgumentsValidator.NotNull(state, "state");
            ArgumentsValidator.NotNull(@object, "object");
            ArgumentsValidator.NotNull(inventory, "inventory");
            ArgumentsValidator.NotNull(owner, "owner");
            ArgumentsValidator.NotNull(workingGroup, "workingGroup");
            ArgumentsValidator.NotNull(administrator, "administrator");
            ArgumentsValidator.NotNull(finishingDate, "finishingDate");
            ArgumentsValidator.NotNull(rss, "rss");

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

        public FieldSettingDto Priority { get; private set; }

        public FieldSettingDto Title { get; private set; }

        public FieldSettingDto State { get; private set; }

        public FieldSettingDto System { get; private set; }

        public FieldSettingDto Object { get; private set; }

        public FieldSettingDto Inventory { get; private set; }

        public FieldSettingDto Owner { get; private set; }

        public FieldSettingDto WorkingGroup { get; private set; }

        public FieldSettingDto Administrator { get; private set; }

        public FieldSettingDto FinishingDate { get; private set; }

        public FieldSettingDto Rss { get; private set; }
    }
}
