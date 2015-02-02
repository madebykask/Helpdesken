namespace DH.Helpdesk.BusinessData.Models.Reports.Options
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistratedCasesDayOptions
    {
        public RegistratedCasesDayOptions(
                    List<ItemOverview> departments, 
                    List<ItemOverview> caseTypes, 
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
        public List<ItemOverview> CaseTypes { get; private set; }
 
        [NotNull]
        public List<ItemOverview> WorkingGroups { get; private set; }
 
        [NotNull]
        public List<ItemOverview> Administrators { get; private set; }
    }
}