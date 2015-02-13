namespace DH.Helpdesk.BusinessData.Models.Reports.Options
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.BusinessData.Models.Shared;

    public sealed class ReportGeneratorOptions
    {
        public ReportGeneratorOptions(
                List<ItemOverview> fields, 
                List<ItemOverview> departments, 
                List<ItemOverview> workingGroups, 
                List<CaseTypeItem> caseTypes)
        {
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
            this.Fields = fields;
        }

        public List<ItemOverview> Fields { get; private set; } 

        public List<ItemOverview> Departments { get; private set; }

        public List<ItemOverview> WorkingGroups { get; private set; }

        public List<CaseTypeItem> CaseTypes { get; private set; } 
    }
}