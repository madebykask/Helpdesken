using System.Web.Mvc;
using DH.Helpdesk.Web.Areas.Reports.Models.Options.ReportGenerator;

namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{
	using System.IO;
	using System.Collections.Generic;

	using DH.Helpdesk.BusinessData.Models.ReportService;
	using DH.Helpdesk.BusinessData.Models.Shared;
	using Microsoft.Reporting.WebForms;
	using BusinessData.Models.Case;

	public sealed class ReportServiceOverviewModel
    {   
        public ReportServiceOverviewModel()
        {
            ReportFavorites = new List<SavedReportFavoriteItemModel>();
        }

        public int CustomerId { get; set; }

        public CustomSelectList ReportList { get; set; }

        public List<SavedReportFavoriteItemModel> ReportFavorites { get; set; }

        public ReportFilterModel ReportFilter { get; set; }

        public ReportPresentationModel ReportViewerData { get; set; }

        public ReportGeneratorOptionsModel ReportGeneratorOptions { get; set; }
		public List<ExtendedCaseCaseSolutionModel> ReportGeneratorExtendedCaseForms { get; internal set; }

		public int ReportGeneratorExtendedCaseFormId { get; internal set; }
		public List<ExtendedCaseFormFieldTranslationModel> ReportGeneratorExtendedCaseFormFields { get; internal set; }
	}

    public sealed class ReportFilterModel : ReportFilter
    {
        public ReportSelectedFilter Selected { get; set; }

        public bool FirstUserNameOrientation { get; set; }
        public List<SelectListItem> StackByList { get; set; }
        public List<SelectListItem> GroupByList { get; set; }
        public DateToDate CaseChangeDate { get; set; }
    }

    public sealed class ReportPresentationModel
    {
        public ReportPresentationModel()
        {

        }
        public ReportViewer ReportPage { get; set; }
    }

    public sealed class SavedReportFavoriteItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OriginalReportId { get; set; }
    }
       
}