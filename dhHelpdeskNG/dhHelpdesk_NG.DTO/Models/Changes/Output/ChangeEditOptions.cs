namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeEditOptions
    {
        public ChangeEditOptions(
            List<ItemOverviewDto> departments,
            List<ItemOverviewDto> statuses,
            List<ItemOverviewDto> systems,
            List<ItemOverviewDto> objects,
            List<GroupWithEmails> workingGroups,
            List<ItemOverviewDto> administrators,
            List<ItemOverviewDto> owners,
            List<ItemOverviewDto> processesAffected,
            List<ItemOverviewDto> categories,
            List<ItemOverviewDto> relatedChanges,
            List<ItemOverviewDto> priorities,
            List<ItemOverviewDto> responsibles,
            List<ItemOverviewDto> currencies,
            List<GroupWithEmails> emailGroups, 
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
            this.RelatedChanges = relatedChanges;
            this.Priorities = priorities;
            this.Responsibles = responsibles;
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
        public List<ItemOverviewDto> Administrators { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Owners { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> ProcessesAffected { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Categories { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> RelatedChanges { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Priorities { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Responsibles { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> Currencies { get; private set; }

        [NotNull]
        public List<GroupWithEmails> EmailGroups { get; private set; }

        [NotNull]
        public List<ItemOverviewDto> ImplementationStatuses { get; private set; }
    }
}
