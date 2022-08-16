namespace DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator
{
    using System;
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Enums.Case.Fields;
    using DH.Helpdesk.BusinessData.Models.ReportService;
    using DH.Helpdesk.BusinessData.Models.Shared;
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
            this.ProductAreaIds = new List<int>();
            this.AdministratorIds = new List<int>();
            this.CaseStatusIds = new List<int>();
            this.CaseTypeIds = new List<int>();
			this.ExtendedCaseFormFieldIds = new List<string>();
        }

        public ReportGeneratorFilterModel(
            List<int> fieldIds, 
            List<int> departmentIds, 
            List<int> workingGroupIds,
            List<int> productAreaIds,
            List<int> administratorIds,
            List<int> caseStatusIds,
            List<int> caseTypeIds,
			List<string> extendedCaseFormFieldIds,
			int? extendedCaseFormId,
            DateTime? periodFrom, 
            DateTime? periodUntil, 
            int recordsOnPage,
            SortField sortField,
            DateTime? closeFrom,
            DateTime? closeTo)
        {
            this.SortField = sortField;
            this.PeriodUntil = periodUntil;
            this.PeriodFrom = periodFrom;
            this.CaseTypeIds = caseTypeIds;
            this.WorkingGroupIds = workingGroupIds;
            this.ProductAreaIds = productAreaIds;
            this.AdministratorIds = administratorIds;
            this.CaseStatusIds = caseStatusIds;
            this.DepartmentIds = departmentIds;
            this.FieldIds = fieldIds;
            this.RecordsOnPage = recordsOnPage;
            this.CloseFrom = closeFrom;
            this.CloseTo = closeTo;
			this.ExtendedCaseFormFieldIds = extendedCaseFormFieldIds;
			this.ExtendedCaseFormId = extendedCaseFormId;
        }

        [NotNull]
        public List<int> FieldIds { get; private set; }

        [NotNull]
        public List<int> DepartmentIds { get; private set; }

        [NotNull]
        public List<int> WorkingGroupIds { get; private set; }

        [NotNull]
        public List<int> ProductAreaIds { get; private set; }

        [NotNull]
        public List<int> AdministratorIds { get; private set; }

        [NotNull]
        public List<int> CaseStatusIds { get; private set; }

        [NotNull]
        public List<int> CaseTypeIds { get; private set; }

        public DateTime? PeriodFrom { get; private set; }

        public DateTime? PeriodUntil { get; private set; }

        public DateTime? CloseFrom { get; private set; }

        public DateTime? CloseTo { get; private set; }

        [MinValue(0)]
        public int RecordsOnPage { get; private set; }

        public SortField SortField { get; private set; }
		public List<string> ExtendedCaseFormFieldIds { get; private set; }
		public int? ExtendedCaseFormId { get; private set; }

        public  ReportSelectedFilter MapToSelectedFilter(ReportGeneratorFilterModel reportFilter)
        {


            var ret = new ReportSelectedFilter();
            //ret.SelectedCustomers.AddItems(reportFilter.SelectedCustomers);
            ret.SeletcedDepartments.AddItems(reportFilter.DepartmentIds);
            //ret.SeletcedOUs.AddItems(reportFilter.SeletcedOUs);
            ret.SelectedWorkingGroups.AddItems(reportFilter.WorkingGroupIds);
            ret.SelectedAdministrator.AddItems(reportFilter.AdministratorIds);
            ret.SelectedCaseTypes.AddItems(reportFilter.CaseTypeIds);
            ret.SelectedProductAreas.AddItems(reportFilter.ProductAreaIds);
            ret.SelectedCaseStatus.AddItems(reportFilter.CaseStatusIds);
            ret.CaseCreationDate = new DateToDate(reportFilter.PeriodFrom, reportFilter.PeriodUntil);
            ret.CaseClosingDate = new DateToDate(reportFilter.CloseFrom, reportFilter.CloseTo);
            //ret.SelectedReportCategory.AddItems(reportFilter.SelectedReportCategory);
            //ret.SelectedReportCategoryRt.AddItems(reportFilter.SelectedReportCategoryRt);
            return ret;
        }

        public static ReportGeneratorFilterModel CreateDefault()
        {
            return new ReportGeneratorFilterModel
                       {
                           SortField = new SortField(CaseInfoFields.Case, SortBy.Ascending),
                           RecordsOnPage = 100,
                           PeriodFrom = DateTime.Today,
                           PeriodUntil = DateTime.Today
                       };
        }


    }
}