namespace DH.Helpdesk.BusinessData.Models.Reports.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class AverageSolutionTimeOptionsResponse
    {
        public AverageSolutionTimeOptionsResponse()
        {
            this.Departments = new ItemOverview[0];
            this.CaseTypes = new ItemOverview[0];
            this.WorkingGroups = new ItemOverview[0];
        }

        public AverageSolutionTimeOptionsResponse(
                IEnumerable<ItemOverview> departments, 
                IEnumerable<ItemOverview> caseTypes, 
                IEnumerable<ItemOverview> workingGroups)
        {
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
    }
}