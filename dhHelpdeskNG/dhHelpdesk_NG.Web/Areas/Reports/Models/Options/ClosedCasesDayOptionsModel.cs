namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class ClosedCasesDayOptionsModel
    {
        public ClosedCasesDayOptionsModel()
        {
            this.DepartmentIds = new List<int>();
        }

        public ClosedCasesDayOptionsModel(
            MultiSelectList departments,
            SelectList workingGroups, 
            List<CaseTypeItem> caseTypes, 
            SelectList administrators,
            DateTime period)
        {
            this.Administrators = administrators;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
            this.Period = period;
        }

        [NotNull]
        public MultiSelectList Departments { get; private set; }

        [LocalizedDisplay("Avdelning påverkad")]
        public List<int> DepartmentIds { get; set; }

        [NotNull]
        public SelectList WorkingGroups { get; private set; }

        [LocalizedDisplay("Driftgrupp")]
        public int? WorkingGroupId { get; set; }

        [NotNull]
        public List<CaseTypeItem> CaseTypes { get; private set; }

        [IsId]
        [LocalizedDisplay("Ärendetyp")]
        public int? CaseTypeId { get; set; }

        [NotNull]
        public SelectList Administrators { get; private set; }

        [LocalizedDisplay("Handläggare")]
        public int? AdministratorId { get; set; }

        [LocalizedRequired]
        [LocalizedDisplay("Period")]
        public DateTime Period { get; set; }
    }
}