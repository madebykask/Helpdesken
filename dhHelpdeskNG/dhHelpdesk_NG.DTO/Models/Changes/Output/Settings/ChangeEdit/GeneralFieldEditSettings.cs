namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralFieldEditSettings
    {
        public GeneralFieldEditSettings(
            FieldEditSetting priority,
            FieldEditSetting title,
            FieldEditSetting status,
            FieldEditSetting system,
            FieldEditSetting @object,
            FieldEditSetting inventory,
            FieldEditSetting workingGroup,
            FieldEditSetting administrator,
            FieldEditSetting finishingDate,
            FieldEditSetting rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.Status = status;
            this.System = system;
            this.Object = @object;
            this.Inventory = inventory;
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
        public FieldEditSetting Status { get; private set; }

        [NotNull]
        public FieldEditSetting System { get; private set; }

        [NotNull]
        public FieldEditSetting Object { get; private set; }

        [NotNull]
        public FieldEditSetting Inventory { get; private set; }

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
