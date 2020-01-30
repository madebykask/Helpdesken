using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports.ReportGenerator
{
	public class ExtendedCaseFormFieldsModel
	{
		public List<ExtendedCaseFormFieldModel> Fields { get; set; }
	}

	public class ExtendedCaseFormFieldModel
	{
		public string ID { get; set; }
		public string Name { get; set; }
	}
}