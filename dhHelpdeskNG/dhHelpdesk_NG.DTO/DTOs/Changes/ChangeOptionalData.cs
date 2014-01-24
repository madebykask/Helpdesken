namespace dhHelpdesk_NG.DTO.DTOs.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public sealed class ChangeOptionalData
    {
        public ChangeOptionalData(
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> statuses,
            List<ItemOverviewDto> systems,
            List<ItemOverviewDto> objects,
            List<ItemOverviewDto> workingGroups,
            List<ItemOverviewDto> administrators,
            List<ItemOverviewDto> owners,
            List<ItemOverviewDto> processesAffected,
            List<ItemOverviewDto> categories,
            List<ItemOverviewDto> priorities,
            List<ItemOverviewDto> responsibles,
            List<ItemOverviewDto> currencies,
            List<ItemOverviewDto> implementationStatuses)
        {
            this.Departments = departments;
            this.Statuses = statuses;
            this.Systems = systems;
            this.Objects = objects;
            this.WorkingGroups = workingGroups;
            this.Administrators = administrators;
            this.Owners = owners;
            this.ProcessesAffected = processesAffected;
            this.Categories = categories;
            this.Priorities = priorities;
            this.Responsibles = responsibles;
            this.Currencies = currencies;
            this.ImplementationStatuses = implementationStatuses;
        }

        [NotNull]
        public List<ItemOverviewDto> Departments { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Statuses { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Systems { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Objects { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Administrators { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Owners { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> ProcessesAffected { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Categories { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Priorities { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Responsibles { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Currencies { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> ImplementationStatuses { get; private set; }
    }
}
