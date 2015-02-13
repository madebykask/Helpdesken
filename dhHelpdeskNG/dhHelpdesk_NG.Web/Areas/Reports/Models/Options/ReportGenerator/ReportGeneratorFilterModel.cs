namespace DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Case.Fields;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ReportGeneratorFilterModel
    {
        public ReportGeneratorFilterModel()
        {
            this.FieldIds = new List<int>();
            this.DepartmentIds = new List<int>();
            this.WorkingGroupIds = new List<int>();
        }

        public ReportGeneratorFilterModel(
            List<int> fieldIds, 
            List<int> departmentIds, 
            List<int> workingGroupIds, 
            int? caseTypeId, 
            DateTime? periodFrom, 
            DateTime? periodUntil, 
            int recordsOnPage,
            SortField sortField)
        {
            this.SortField = sortField;
            this.PeriodUntil = periodUntil;
            this.PeriodFrom = periodFrom;
            this.CaseTypeId = caseTypeId;
            this.WorkingGroupIds = workingGroupIds;
            this.DepartmentIds = departmentIds;
            this.FieldIds = fieldIds;
            this.RecordsOnPage = recordsOnPage;
        }

        [NotNull]
        public List<int> FieldIds { get; private set; }

        [NotNull]
        public List<int> DepartmentIds { get; private set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; private set; }

        public int? CaseTypeId { get; private set; }

        public DateTime? PeriodFrom { get; private set; }

        public DateTime? PeriodUntil { get; private set; }

        [MinValue(0)]
        public int RecordsOnPage { get; private set; }

        public SortField SortField { get; private set; }

        public static ReportGeneratorFilterModel CreateDefault()
        {
            return new ReportGeneratorFilterModel
                       {
                           SortField = new SortField(CaseInfoFields.Case, SortBy.Ascending),
                           RecordsOnPage = 100,
                           PeriodFrom = DateTime.Today.AddMonths(-1),
                           PeriodUntil = DateTime.Today
                       };
        }
    }
}