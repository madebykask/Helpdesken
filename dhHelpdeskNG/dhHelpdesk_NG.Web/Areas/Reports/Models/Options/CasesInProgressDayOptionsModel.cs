namespace DH.Helpdesk.Web.Areas.Reports.Models.Options
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class CasesInProgressDayOptionsModel
    {
        public CasesInProgressDayOptionsModel(
            SelectList departments,
            SelectList workingGroups, 
            SelectList administrators,
            DateTime period)
        {
            this.Administrators = administrators;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
            this.Period = period;
        }

        [NotNull]
        public SelectList Departments { get; private set; }

        [LocalizedDisplay("Avdelning påverkad")]
        public int? DepartmentId { get; set; }

        [NotNull]
        public SelectList WorkingGroups { get; private set; }

        [LocalizedDisplay("Driftgrupp")]
        public int? WorkingGroupId { get; set; }

        [NotNull]
        public SelectList Administrators { get; private set; }

        [LocalizedDisplay("Handläggare")]
        public int? AdministratorId { get; set; }

        [LocalizedRequired]
        [LocalizedDisplay("Period")]
        public DateTime Period { get; set; }
    }
}