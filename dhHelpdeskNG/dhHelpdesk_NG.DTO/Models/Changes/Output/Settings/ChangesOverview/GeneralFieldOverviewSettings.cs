namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangesOverview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralFieldOverviewSettings
    {
        public GeneralFieldOverviewSettings(
            FieldOverviewSetting priority,
            FieldOverviewSetting title,
            FieldOverviewSetting state,
            FieldOverviewSetting system,
            FieldOverviewSetting @object,
            FieldOverviewSetting inventory,
            FieldOverviewSetting owner,
            FieldOverviewSetting workingGroup,
            FieldOverviewSetting administrator,
            FieldOverviewSetting finishingDate,
            FieldOverviewSetting rss)
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
        public FieldOverviewSetting Priority { get; private set; }

        [NotNull]
        public FieldOverviewSetting Title { get; private set; }

        [NotNull]
        public FieldOverviewSetting State { get; private set; }

        [NotNull]
        public FieldOverviewSetting System { get; private set; }

        [NotNull]
        public FieldOverviewSetting Object { get; private set; }

        [NotNull]
        public FieldOverviewSetting Inventory { get; private set; }

        [NotNull]
        public FieldOverviewSetting Owner { get; private set; }

        [NotNull]
        public FieldOverviewSetting WorkingGroup { get; private set; }

        [NotNull]
        public FieldOverviewSetting Administrator { get; private set; }

        [NotNull]
        public FieldOverviewSetting FinishingDate { get; private set; }

        [NotNull]
        public FieldOverviewSetting Rss { get; private set; }
    }
}
