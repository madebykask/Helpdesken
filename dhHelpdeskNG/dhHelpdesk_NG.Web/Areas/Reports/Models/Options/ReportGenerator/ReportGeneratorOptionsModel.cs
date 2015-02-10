namespace DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.CaseType;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class ReportGeneratorOptionsModel
    {
        public ReportGeneratorOptionsModel()
        {
            this.FieldIds = new List<int>();
            this.DepartmentIds = new List<int>();
            this.WorkingGroupIds = new List<int>();            
        }

        public ReportGeneratorOptionsModel(
                MultiSelectList fields, 
                MultiSelectList departments, 
                MultiSelectList workingGroups, 
                List<CaseTypeItem> caseTypes,
                DateTime periodFrom,
                DateTime periodUntil, 
                int recordsOnPage,
                SortFieldModel sortField)
        {
            this.SortField = sortField;
            this.RecordsOnPage = recordsOnPage;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
            this.Fields = fields;
            this.PeriodFrom = periodFrom;
            this.PeriodUntil = periodUntil;
        }

        [NotNull]
        public MultiSelectList Fields { get; private set; }

        [NotNull]
        [LocalizedDisplay("Fält")]
        public List<int> FieldIds { get; set; }

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

        public bool IsExcel { get; set; }

        [LocalizedDisplay("poster per sida")]
        [LocalizedInteger]
        [LocalizedMin(0)]
        public int RecordsOnPage { get; set; }

        public SortFieldModel SortField { get; set; }

        public ReportGeneratorFilterModel GetFilter()
        {
            SortField sortField = null;

            if (!string.IsNullOrEmpty(this.SortField.Name) && this.SortField.SortBy != null)
            {
                sortField = new SortField(this.SortField.Name, this.SortField.SortBy.Value);
            }

            return new ReportGeneratorFilterModel(
                        this.FieldIds,
                        this.DepartmentIds,
                        this.WorkingGroupIds,
                        this.CaseTypeId,
                        this.PeriodFrom,
                        this.PeriodUntil,
                        this.RecordsOnPage,
                        sortField);
        }
    }
}