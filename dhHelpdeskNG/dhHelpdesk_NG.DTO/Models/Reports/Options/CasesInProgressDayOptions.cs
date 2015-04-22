namespace DH.Helpdesk.BusinessData.Models.Reports.Options
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CasesInProgressDayOptions
    {
        public CasesInProgressDayOptions(
            List<ItemOverview> departments, 
            List<ItemOverview> workingGroups, 
            List<ItemOverview> administrators)
        {
            this.Administrators = administrators;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
        }

        [NotNull]
        public List<ItemOverview> Departments { get; private set; }

        [NotNull]
        public List<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> Administrators { get; private set; }
    }
}