namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class RegistratedCasesDayOptionsResponse
    {
        public RegistratedCasesDayOptionsResponse()
        {
            this.Departments = new ItemOverview[0];
            this.CaseTypes = new ItemOverview[0];
            this.WorkingGroups = new ItemOverview[0];
            this.Administrators = new ItemOverview[0];
        }

        public RegistratedCasesDayOptionsResponse(
                IEnumerable<ItemOverview> departments, 
                IEnumerable<ItemOverview> caseTypes, 
                IEnumerable<ItemOverview> workingGroups, 
                IEnumerable<ItemOverview> administrators)
        {
            this.Administrators = administrators;
            this.WorkingGroups = workingGroups;
            this.CaseTypes = caseTypes;
            this.Departments = departments;
        }

        [NotNull]
        public IEnumerable<ItemOverview> Departments { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> CaseTypes { get; private set; }

        [NotNull]
        public IEnumerable<ItemOverview> WorkingGroups { get; private set; } 

        [NotNull]
        public IEnumerable<ItemOverview> Administrators { get; private set; } 
    }
}