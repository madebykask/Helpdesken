using System.Linq;
using DH.Helpdesk.Common.Enums;

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
	using BusinessData.Models.Reports.Enums;

	public sealed class ReportGeneratorOptionsModel
    {
        public ReportGeneratorOptionsModel()
        {
            this.FieldIds = new List<int>();
			this.ExtendedCaseFormFieldIds = new List<string>();
			this.DepartmentIds = new List<int>();
            this.WorkingGroupIds = new List<int>();
            this.CaseTypeIds = new List<int>();   
            this.AdministratorsIds = new List<int>();
            this.ProductAreaIds = new List<int>();
        }

        public ReportGeneratorOptionsModel(
			    
                MultiSelectList fields, 
                MultiSelectList departments, 
                MultiSelectList workingGroups, 
                List<CaseTypeItem> caseTypes,
                List<int> caseTypeIds,
                DateTime periodFrom,
                DateTime periodUntil, 
                int recordsOnPage,
                string sortName,
                 SortBy? sortBy,
                 DateTime? closeFrom,
                 DateTime? closeTo)
        {
            this.SortName = sortName;
            this.SortBy = sortBy;
            this.RecordsOnPage = recordsOnPage;
            this.CaseTypes = caseTypes;
            this.WorkingGroups = workingGroups;
            this.Departments = departments;
            this.Fields = fields;
            this.PeriodFrom = periodFrom;
            this.PeriodUntil = periodUntil;
            this.CaseTypeIds = caseTypeIds;
            this.CloseFrom = closeFrom;
            this.CloseTo = closeTo;
        }

        //[NotNull]
        public MultiSelectList Fields { get; private set; }

        [NotNull]
        [LocalizedDisplay("Fält")]
        public List<int> FieldIds { get; set; }

		public List<string> ExtendedCaseFormFieldIds { get; set; }

		public int? ExtendedCaseFormId { get; set; }


		//[NotNull]
		public MultiSelectList Departments { get; private set; }

        [LocalizedDisplay("Avdelning påverkad")]
        public List<int> DepartmentIds { get; set; }

        //[NotNull]
        public MultiSelectList WorkingGroups { get; private set; }

        [LocalizedDisplay("Driftgrupp")]
        public List<int> WorkingGroupIds { get; set; }

        //[NotNull]        
        public List<CaseTypeItem> CaseTypes { get; private set; }

        [LocalizedDisplay("Ärendetyp")]
        public List<int> CaseTypeIds { get; set; }

        [LocalizedDisplay("Handläggare")]
        public List<int> AdministratorsIds { get; set; }

        [LocalizedDisplay("Status")]
        public int? CaseStatusIds { get; set; }

        [LocalizedDisplay("Produktområde")]
        public List<int> ProductAreaIds { get; set; }

        [LocalizedDisplay("Period från")]
        public DateTime? PeriodFrom { get; set; }

        [LocalizedDisplay("Period till")]
        public DateTime? PeriodUntil { get; set; }

//        [LocalizedDisplay("Period från")]
        public DateTime? CloseFrom { get; set; }

//        [LocalizedDisplay("Period till")]
        public DateTime? CloseTo { get; set; }

        public bool IsExcel { get; set; }

        public bool IsPreview { get; set; }

        [LocalizedDisplay("poster per sida")]
        [LocalizedInteger]
        [LocalizedMin(0)]
        public int RecordsOnPage { get; set; }

        public string SortName { get; set; }
        public SortBy? SortBy { get; set; }
		public List<string> ExtendedCaseTranslationFieldIds { get; internal set; }
		public int ReportTypeId { get; set; }

		public ReportGeneratorFilterModel GetFilter()
        {
            SortField sortField = null;

            if (!string.IsNullOrEmpty(SortName) && SortBy.HasValue)
            {
                sortField = new SortField(this.SortName, SortBy.Value);
            }

            return new ReportGeneratorFilterModel(
                        this.FieldIds,
                        this.DepartmentIds,
                        this.WorkingGroupIds,
                        this.ProductAreaIds,
                        this.AdministratorsIds,
                        (this.CaseStatusIds == null || this.CaseStatusIds < 0) ? new List<int>() : new List<int> { this.CaseStatusIds.Value }, //service supports multiple statuses but ui not - converting ui input to service supported list
                        this.CaseTypeIds,
						this.ReportTypeId == (int)ReportType.ReportGeneratorExtendedCase ? this.ExtendedCaseFormFieldIds : null,
						this.ReportTypeId == (int)ReportType.ReportGeneratorExtendedCase ? this.ExtendedCaseFormId : null,
                        this.PeriodFrom,
                        this.PeriodUntil,
                        this.RecordsOnPage,
                        sortField,
                        CloseFrom,
                        CloseTo);
        }
    }
}