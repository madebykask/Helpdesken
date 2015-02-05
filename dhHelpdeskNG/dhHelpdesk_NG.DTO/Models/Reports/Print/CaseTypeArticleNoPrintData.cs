namespace DH.Helpdesk.BusinessData.Models.Reports.Print
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo;

    public sealed class CaseTypeArticleNoPrintData
    {
        public CaseTypeArticleNoPrintData(
                List<string> departments, 
                List<string> workingGroups,                 
                List<string> caseTypes, 
                List<string> productAreas,
                CaseTypeArticleNoData data)
        {
            this.ProductAreas = productAreas;
            this.CaseTypes = caseTypes;
            this.Data = data;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
        }

        public List<string> Departments { get; private set; }
        
        public List<string> WorkingGroups { get; private set; }

        public List<string> CaseTypes { get; private set; }

        public List<string> ProductAreas { get; private set; }

        public CaseTypeArticleNoData Data { get; private set; }
    }
}