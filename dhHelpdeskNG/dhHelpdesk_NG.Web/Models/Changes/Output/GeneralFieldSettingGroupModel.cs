namespace dhHelpdesk_NG.Web.Models.Changes.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

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
        public FieldSettingModel Priority { get; private set; }

        [NotNull]
        public FieldSettingModel Title { get; private set; }

        [NotNull]
        public FieldSettingModel State { get; private set; }

        [NotNull]
        public FieldSettingModel System { get; private set; }

        [NotNull]
        public FieldSettingModel Object { get; private set; }

        [NotNull]
        public FieldSettingModel Inventory { get; private set; }

        [NotNull]
        public FieldSettingModel Owner { get; private set; }

        [NotNull]
        public FieldSettingModel WorkingGroup { get; private set; }

        [NotNull]
        public FieldSettingModel Administrator { get; private set; }

        [NotNull]
        public FieldSettingModel FinishingDate { get; private set; }

        [NotNull]
        public FieldSettingModel Rss { get; private set; }
    }
}
