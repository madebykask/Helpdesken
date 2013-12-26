namespace dhHelpdesk_NG.DTO.DTOs.Changes.Input
{
    using dhHelpdesk_NG.Common.Tools;

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
            ArgumentsValidator.NotNull(priority, "priority");
            ArgumentsValidator.NotNull(title, "title");
            ArgumentsValidator.NotNull(state, "state");
            ArgumentsValidator.NotNull(system, "system");
            ArgumentsValidator.NotNull(@object, "@object");
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

        public UpdatedFieldSettingDto Priority { get; private set; }

        public UpdatedFieldSettingDto Title { get; private set; }

        public UpdatedFieldSettingDto State { get; private set; }

        public UpdatedFieldSettingDto System { get; private set; }

        public UpdatedFieldSettingDto Object { get; private set; }

        public UpdatedFieldSettingDto Inventory { get; private set; }

        public UpdatedFieldSettingDto Owner { get; private set; }

        public UpdatedFieldSettingDto WorkingGroup { get; private set; }

        public UpdatedFieldSettingDto Administrator { get; private set; }

        public UpdatedFieldSettingDto FinishingDate { get; private set; }

        public UpdatedFieldSettingDto Rss { get; private set; }
    }
}
