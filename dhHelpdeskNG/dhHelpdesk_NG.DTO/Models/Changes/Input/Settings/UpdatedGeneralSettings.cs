namespace DH.Helpdesk.BusinessData.Models.Changes.Input.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class UpdatedGeneralSettings
    {
        public UpdatedGeneralSettings(
            UpdatedFieldSetting priority,
            UpdatedFieldSetting title,
            UpdatedFieldSetting state,
            UpdatedFieldSetting system,
            UpdatedFieldSetting @object,
            UpdatedFieldSetting inventory,
            UpdatedFieldSetting workingGroup,
            UpdatedFieldSetting administrator,
            UpdatedFieldSetting finishingDate,
            UpdatedFieldSetting rss)
        {
            this.Priority = priority;
            this.Title = title;
            this.State = state;
            this.System = system;
            this.Object = @object;
            this.Inventory = inventory;
            this.WorkingGroup = workingGroup;
            this.Administrator = administrator;
            this.FinishingDate = finishingDate;
            this.Rss = rss;
        }

        [NotNull]
        public UpdatedFieldSetting Priority { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Title { get; private set; }

        [NotNull]
        public UpdatedFieldSetting State { get; private set; }

        [NotNull]
        public UpdatedFieldSetting System { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Object { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Inventory { get; private set; }

        [NotNull]
        public UpdatedFieldSetting WorkingGroup { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Administrator { get; private set; }

        [NotNull]
        public UpdatedFieldSetting FinishingDate { get; private set; }

        [NotNull]
        public UpdatedFieldSetting Rss { get; private set; }
    }
}
