using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Reports.Models.ReportService
{
	public class ExtendedCaseFormFieldTranslationModel
	{
		public string FieldId { get; set; }
		public int LanguageId { get; set; }
		public string Text { get; set; }
	}
}