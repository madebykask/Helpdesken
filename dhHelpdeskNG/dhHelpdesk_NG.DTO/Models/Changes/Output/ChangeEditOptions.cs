namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeEditOptions
    {
        public ChangeEditOptions(
            List<ItemOverview> departments,
            List<ItemOverview> statuses,
            List<ItemOverview> systems,
            List<ItemOverview> objects,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators,
            List<ItemOverview> owners,
            List<ItemOverview> processesAffected,
            List<ItemOverview> categories,
            List<ItemOverview> relatedChanges,
            List<ItemOverview> priorities,
            List<ItemOverview> responsibles,
            List<ItemOverview> currencies,
            List<GroupWithEmails> emailGroups, 
            List<ItemOverview> implementationStatuses)
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
        public List<ItemOverview> Departments { get; private set; }

        [NotNull]
        public List<ItemOverview> Statuses { get; private set; }

        [NotNull]
        public List<ItemOverview> Systems { get; private set; }

        [NotNull]
        public List<ItemOverview> Objects { get; private set; }

        [NotNull]
        public List<GroupWithEmails> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> Administrators { get; private set; }

        [NotNull]
        public List<ItemOverview> Owners { get; private set; }

        [NotNull]
        public List<ItemOverview> ProcessesAffected { get; private set; }

        [NotNull]
        public List<ItemOverview> Categories { get; private set; }

        [NotNull]
        public List<ItemOverview> RelatedChanges { get; private set; }

        [NotNull]
        public List<ItemOverview> Priorities { get; private set; }

        [NotNull]
        public List<ItemOverview> Responsibles { get; private set; }

        [NotNull]
        public List<ItemOverview> Currencies { get; private set; }

        [NotNull]
        public List<GroupWithEmails> EmailGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> ImplementationStatuses { get; private set; }
    }
}
