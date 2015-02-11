namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class LeadtimeFinishedCasesOptionsModel
    {
        public LeadtimeFinishedCasesOptionsModel()
        {
            this.DepartmentIds = new List<int>();
            this.CaseTypes = new List<CaseTypeItem>();   
            this.WorkingGroupIds = new List<int>();   
        }

        public LeadtimeFinishedCasesOptionsModel(
            MultiSelectList departments, 
            List<CaseTypeItem> caseTypes,
            int? caseTypeId,
            MultiSelectList workingGroups, 
            SelectList registrationSources,
            DateTime periodFrom,
            DateTime periodUntil, 
            SelectList leadTimes,
            bool isShowDetails)
        {
            this.LeadTimes = leadTimes;
            this.RegistrationSources = registrationSources;
            this.WorkingGroups = workingGroups;
            this.CaseTypes = caseTypes;
            this.Departments = departments;
            this.CaseTypeId = caseTypeId;
            this.PeriodFrom = periodFrom;
            this.PeriodUntil = periodUntil;
            this.IsShowDetails = isShowDetails;
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

        [NotNull]
        public MultiSelectList WorkingGroups { get; private set; }

        [LocalizedDisplay("Driftgrupp")]
        public List<int> WorkingGroupIds { get; set; }

        [NotNull]
        public SelectList RegistrationSources { get; private set; }

        [LocalizedDisplay("Källa registrering")]
        public int RegistrationSourceId { get; set; }

        [LocalizedDisplay("Period från")]
        public DateTime? PeriodFrom { get; set; }

        [LocalizedDisplay("Period till")]
        public DateTime? PeriodUntil { get; set; }

        [NotNull]
        public SelectList LeadTimes { get; private set; }

        [IsId]
        [LocalizedDisplay("Ledtid")]
        public int LeadTimeId { get; set; }

        [LocalizedDisplay("Show Report details")]
        public bool IsShowDetails { get; set; }
    }
}