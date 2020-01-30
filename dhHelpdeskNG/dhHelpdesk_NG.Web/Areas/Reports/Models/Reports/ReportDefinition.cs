using DH.Helpdesk.BusinessData.Models.Reports.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Reports.Models.Reports
{
	public class ReportDefinition
	{
		public ReportDefinition(string identifier, string name, ReportType reportType)
		{
			Identifier = identifier;
			Name = name;
			ReportType = reportType;
		}
		public string Name { get; set; }
		public string Identifier { get; set; }
		public ReportType ReportType { get; set; }
	}
}