namespace DH.Helpdesk.Web.Models.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class AverageSolutionTimeOptions
    {
        public AverageSolutionTimeOptions()
        {
            this.CaseTypeIds = new List<int>();
        }

        public AverageSolutionTimeOptions(
                SelectList departments, 
                MultiSelectList caseTypes, 
                SelectList workingGroups, 
                DateTime periodFrom, 
                DateTime periodUntil)
        {
            this.PeriodUntil = periodUntil;
            this.PeriodFrom = periodFrom;
            this.WorkingGroups = workingGroups;
            this.CaseTypes = caseTypes;
            this.Departments = departments;
        }

        [NotNull]
        public SelectList Departments { get; private set; }

        public int? DepartmentId { get; set; }

        [NotNull]
        public MultiSelectList CaseTypes { get; private set; }

        [NotNull]
        public List<int> CaseTypeIds { get; set; }

        [NotNull]
        public SelectList WorkingGroups { get; private set; }

        public int? WorkingGroupId { get; set; }

        [LocalizedRequired]
        public DateTime PeriodFrom { get; private set; }

        [LocalizedRequired]
        public DateTime PeriodUntil { get; private set; }

        public bool IsPrint { get; set; }
    }
}