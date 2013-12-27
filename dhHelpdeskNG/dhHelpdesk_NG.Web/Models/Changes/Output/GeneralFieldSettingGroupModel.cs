namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class GeneralFieldSettingGroupModel
    {
        public GeneralFieldSettingGroupModel(
            FieldSettingModel priority,
            FieldSettingModel title,
            FieldSettingModel state,
            FieldSettingModel system,
            FieldSettingModel @object,
            FieldSettingModel inventory,
            FieldSettingModel owner,
            FieldSettingModel workingGroup,
            FieldSettingModel administrator,
            FieldSettingModel finishingDate,
            FieldSettingModel rss)
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

        public FieldSettingModel Priority { get; private set; }

        public FieldSettingModel Title { get; private set; }

        public FieldSettingModel State { get; private set; }

        public FieldSettingModel System { get; private set; }

        public FieldSettingModel Object { get; private set; }

        public FieldSettingModel Inventory { get; private set; }

        public FieldSettingModel Owner { get; private set; }

        public FieldSettingModel WorkingGroup { get; private set; }

        public FieldSettingModel Administrator { get; private set; }

        public FieldSettingModel FinishingDate { get; private set; }

        public FieldSettingModel Rss { get; private set; }
    }
}
