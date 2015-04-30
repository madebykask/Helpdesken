namespace DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchSettings
    {
        public SearchSettings(
            FieldOverviewSetting statuses,
            FieldOverviewSetting objects,
            FieldOverviewSetting owners,
            FieldOverviewSetting affectedProcesses,
            FieldOverviewSetting workingGroups,
            FieldOverviewSetting administrators,
            FieldOverviewSetting responsibles)
        {
            this.Statuses = statuses;
            this.Objects = objects;
            this.Owners = owners;
            this.AffectedProcesses = affectedProcesses;
            this.WorkingGroups = workingGroups;
            this.Administrators = administrators;
            this.Responsibles = responsibles;
        }

        [NotNull]
        public FieldOverviewSetting Statuses { get; private set; }

        [NotNull]
        public FieldOverviewSetting Objects { get; private set; }

        [NotNull]
        public FieldOverviewSetting Owners { get; private set; }

        [NotNull]
        public FieldOverviewSetting AffectedProcesses { get; private set; }

        [NotNull]
        public FieldOverviewSetting WorkingGroups { get; private set; }

        [NotNull]
        public FieldOverviewSetting Administrators { get; private set; }

        [NotNull]
        public FieldOverviewSetting Responsibles { get; private set; }
    }
}
