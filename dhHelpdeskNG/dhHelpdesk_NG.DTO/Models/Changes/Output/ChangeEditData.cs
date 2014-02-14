namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeEditData
    {
        internal ChangeEditData(
            List<ItemOverview> departments,
            List<ItemOverview> statuses,
            List<ItemOverview> systems,
            List<ItemOverview> objects,
            List<ItemOverview> workingGroups,
            List<GroupWithEmails> workingGroupsWithEmails,
            List<ItemOverview> administrators,
            List<ItemOverview> owners,
            List<ItemOverview> affectedProcesses,
            List<ItemOverview> affectedDepartments,
            List<ItemOverview> categories,
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
            this.WorkingGroupsWithEmails = workingGroupsWithEmails;
            this.Administrators = administrators;
            this.Owners = owners;
            this.AffectedProcesses = affectedProcesses;
            this.AffectedDepartments = affectedDepartments;
            this.Categories = categories;
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
        public List<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public List<GroupWithEmails> WorkingGroupsWithEmails { get; private set; }

        [NotNull]
        public List<ItemOverview> Administrators { get; private set; }

        [NotNull]
        public List<ItemOverview> Owners { get; private set; }

        [NotNull]
        public List<ItemOverview> AffectedProcesses { get; private set; }

        [NotNull]
        public List<ItemOverview> AffectedDepartments { get; private set; }

        [NotNull]
        public List<ItemOverview> Categories { get; private set; }

        [NotNull]
        public List<ItemOverview> RelatedChanges { get; internal set; }

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
