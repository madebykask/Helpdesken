namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class CaseTypeArticleNoOptionsModel
    {
        public CaseTypeArticleNoOptionsModel(
                MultiSelectList departments, 
                MultiSelectList workingGroups, 
                MultiSelectList caseTypes, 
                List<ProductAreaItem> productAreas,
                DateTime periodFrom,
                DateTime periodUntil, 
                SelectList showCases,
                bool isShowCaseTypeDetails,
                bool isShowPercents)
        {
            this.ShowCases = showCases;
            this.ProductAreas = productAreas;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
            this.PeriodFrom = periodFrom;
            this.PeriodUntil = periodUntil;
            this.IsShowCaseTypeDetails = isShowCaseTypeDetails;
            this.IsShowPercents = isShowPercents;
        }

        [NotNull]
        public MultiSelectList Departments { get; private set; }

        [LocalizedDisplay("Avdelning påverkad")]
        public List<int> DepartmentIds { get; set; }

        [NotNull]
        public MultiSelectList WorkingGroups { get; private set; }

        [LocalizedDisplay("Driftgrupp")]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        public MultiSelectList CaseTypes { get; private set; }

        [NotNull]
        [LocalizedDisplay("Ärendetyp")]
        public List<int> CaseTypeIds { get; set; }

        [NotNull]        
        public List<ProductAreaItem> ProductAreas { get; private set; }

        [IsId]
        [LocalizedDisplay("Produktområde")]
        public int? ProductAreaId { get; set; }

        [LocalizedRequired]
        [LocalizedDisplay("Period från")]
        public DateTime PeriodFrom { get; set; }

        [LocalizedRequired]
        [LocalizedDisplay("Period till")]
        public DateTime PeriodUntil { get; set; }

        [NotNull]
        public SelectList ShowCases { get; private set; }

        [LocalizedRequired]
        [LocalizedDisplay("Visa")]
        public ShowCases ShowCasesId { get; set; }

        [LocalizedDisplay("Visa detaljerad rapport per ärendetyp")]
        public bool IsShowCaseTypeDetails { get; set; }

        [LocalizedDisplay("Visa")]
        public bool IsShowPercents { get; set; }
    }
}