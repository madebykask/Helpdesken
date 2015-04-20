namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class FinishingCauseCategoryCustomerOptionsModel
    {
        public FinishingCauseCategoryCustomerOptionsModel()
        {
            this.DepartmentIds = new List<int>();
            this.WorkingGroupIds = new List<int>();   
        }

        public FinishingCauseCategoryCustomerOptionsModel(
            MultiSelectList departments, 
            MultiSelectList workingGroups, 
            List<CaseTypeItem> caseTypes, 
            DateTime periodFrom,
            DateTime periodUntil)
        {
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
            this.PeriodFrom = periodFrom;
            this.PeriodUntil = periodUntil;
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
        public List<CaseTypeItem> CaseTypes { get; private set; }

        [IsId]
        [LocalizedDisplay("Ärendetyp")]
        public int? CaseTypeId { get; set; }

        [LocalizedDisplay("Period från")]
        public DateTime? PeriodFrom { get; set; }

        [LocalizedDisplay("Period till")]
        public DateTime? PeriodUntil { get; set; }         
    }
}