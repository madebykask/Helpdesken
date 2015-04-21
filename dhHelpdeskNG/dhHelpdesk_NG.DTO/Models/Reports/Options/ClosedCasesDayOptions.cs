namespace DH.Helpdesk.BusinessData.Models.Reports.Options
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ClosedCasesDayOptions
    {
        public ClosedCasesDayOptions(
            List<ItemOverview> departments, 
            List<CaseTypeItem> caseTypes, 
            List<ItemOverview> workingGroups, 
            List<ItemOverview> administrators)
        {
            this.Administrators = administrators;
            this.WorkingGroups = workingGroups;
            this.CaseTypes = caseTypes;
            this.Departments = departments;
        }

        [NotNull]
        public List<ItemOverview> Departments { get; private set; }

        [NotNull]
        public List<CaseTypeItem> CaseTypes { get; private set; }

        [NotNull]
        public List<ItemOverview> WorkingGroups { get; private set; }

        [NotNull]
        public List<ItemOverview> Administrators { get; private set; }
    }
}