namespace DH.Helpdesk.Services.AggregateData.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeEditOptionsData
    {
        public ChangeEditOptionsData(
            List<ItemOverview> departments,
            List<ItemOverview> statuses,
            List<ItemOverview> systems,
            List<ItemOverview> objects,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> users,
            List<ItemOverview> changeGroups,
            List<ItemOverview> categories,
            List<ItemOverview> relatedChanges,
            List<ItemOverview> priorities,
            List<ItemOverview> currencies,
            List<GroupWithEmails> emailGroups,
            List<ItemOverview> implementationStatuses)
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
        public List<ItemOverview> Users { get; private set; }

        [NotNull]
        public List<ItemOverview> ChangeGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> Categories { get; private set; }

        [NotNull]
        public List<ItemOverview> RelatedChanges { get; private set; }

        [NotNull]
        public List<ItemOverview> Priorities { get; private set; }

        [NotNull]
        public List<ItemOverview> Currencies { get; private set; }

        [NotNull]
        public List<GroupWithEmails> EmailGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> ImplementationStatuses { get; private set; }
    }
}
