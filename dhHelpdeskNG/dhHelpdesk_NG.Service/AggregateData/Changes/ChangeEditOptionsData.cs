namespace dhHelpdesk_NG.Service.AggregateData.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public sealed class ChangeEditOptionsData
    {
        public ChangeEditOptionsData(
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> statuses,
            List<ItemOverviewDto> systems,
            List<ItemOverviewDto> objects,
            List<GroupWithEmails> workingGroups,
            List<ItemOverviewDto> users,
            List<ItemOverviewDto> changeGroups,
            List<ItemOverviewDto> categories,
            List<ItemOverviewDto> relatedChanges,
            List<ItemOverviewDto> priorities,
            List<ItemOverviewDto> currencies,
            List<GroupWithEmails> emailGroups,
            List<ItemOverviewDto> implementationStatuses)
        {
            this.Departments = departments;
            this.Statuses = statuses;
            this.Systems = systems;
            this.Objects = objects;
            this.WorkingGroups = workingGroups;
            this.Users = users;
            this.ChangeGroups = changeGroups;
            this.Categories = categories;
            this.RelatedChanges = relatedChanges;
            this.Priorities = priorities;
            this.Currencies = currencies;
            this.EmailGroups = emailGroups;
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
        public List<GroupWithEmails> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Users { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> ChangeGroups { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Categories { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> RelatedChanges { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Priorities { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Currencies { get; private set; }

        [NotNull]
        public List<GroupWithEmails> EmailGroups { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> ImplementationStatuses { get; private set; }
    }
}
