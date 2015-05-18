namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchOptions
    {
        public SearchOptions(
            List<ItemOverview> statuses,
            List<ItemOverview> objects,
            List<ItemOverview> owners,
            List<ItemOverview> affectedProcesses,
            List<ItemOverview> workingGroups,
            List<ItemOverview> administrators, 
            List<ItemOverview> responsibles)
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
        public List<ItemOverview> Statuses { get; private set; }

        [NotNull]
        public List<ItemOverview> Objects { get; private set; }

        [NotNull]
        public List<ItemOverview> Owners { get; private set; }

        [NotNull]
        public List<ItemOverview> AffectedProcesses { get; private set; }

        [NotNull]
        public List<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> Administrators { get; private set; }

        [NotNull]
        public List<ItemOverview> Responsibles { get; private set; }
    }
}
