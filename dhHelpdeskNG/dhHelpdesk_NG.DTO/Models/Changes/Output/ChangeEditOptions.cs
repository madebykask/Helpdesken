namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;

    public sealed class ChangeEditOptions
    {
        internal ChangeEditOptions(
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

        public List<ItemOverview> Departments { get; private set; }

        public List<ItemOverview> Statuses { get; private set; }

        public List<ItemOverview> Systems { get; private set; }

        public List<ItemOverview> Objects { get; private set; }

        public List<ItemOverview> WorkingGroups { get; private set; }

        public List<GroupWithEmails> WorkingGroupsWithEmails { get; private set; }

        public List<ItemOverview> Administrators { get; private set; }

        public List<ItemOverview> Owners { get; private set; }

        public List<ItemOverview> AffectedProcesses { get; private set; }

        public List<ItemOverview> AffectedDepartments { get; private set; }

        public List<ItemOverview> Categories { get; private set; }

        public List<ItemOverview> RelatedChanges { get; internal set; }

        public List<ItemOverview> Priorities { get; private set; }

        public List<ItemOverview> Responsibles { get; private set; }

        public List<ItemOverview> Currencies { get; private set; }

        public List<GroupWithEmails> EmailGroups { get; private set; }

        public List<ItemOverview> ImplementationStatuses { get; private set; }
    }
}
