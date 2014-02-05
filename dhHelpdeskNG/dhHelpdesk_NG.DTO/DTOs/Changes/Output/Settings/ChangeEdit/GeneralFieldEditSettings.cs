namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class GeneralFieldEditSettings
    {
        public GeneralFieldEditSettings(
            FieldEditSetting priority,
            FieldEditSetting title,
            FieldEditSetting state,
            FieldEditSetting system,
            FieldEditSetting @object,
            FieldEditSetting inventory,
            FieldEditSetting owner,
            FieldEditSetting workingGroup,
            FieldEditSetting administrator,
            FieldEditSetting finishingDate,
            FieldEditSetting rss)
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
        public FieldEditSetting Priority { get; private set; }

        [NotNull]
        public FieldEditSetting Title { get; private set; }

        [NotNull]
        public FieldEditSetting State { get; private set; }

        [NotNull]
        public FieldEditSetting System { get; private set; }

        [NotNull]
        public FieldEditSetting Object { get; private set; }

        [NotNull]
        public FieldEditSetting Inventory { get; private set; }

        [NotNull]
        public FieldEditSetting Owner { get; private set; }

        [NotNull]
        public FieldEditSetting WorkingGroup { get; private set; }

        [NotNull]
        public FieldEditSetting Administrator { get; private set; }

        [NotNull]
        public FieldEditSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldEditSetting Rss { get; private set; }
    }
}
