namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LeadtimeActiveCasesOptionsModel
    {
        public LeadtimeActiveCasesOptionsModel()
        {
            this.DepartmentIds = new List<int>();
            this.CaseTypes = new List<CaseTypeItem>();   
        }

        public LeadtimeActiveCasesOptionsModel(
            MultiSelectList departments, 
            List<CaseTypeItem> caseTypes,
            int? caseTypeId)
        {
            this.CaseTypes = caseTypes;
            this.Departments = departments;
            this.CaseTypeId = caseTypeId;
        }

        [NotNull]
        public MultiSelectList Departments { get; private set; }

        [LocalizedDisplay("Avdelning påverkad")]
        public List<int> DepartmentIds { get; set; }

        [NotNull]
        public List<CaseTypeItem> CaseTypes { get; private set; }

        [IsId]
        [LocalizedDisplay("Ärendetyp")]
        public int? CaseTypeId { get; set; }
    }
}