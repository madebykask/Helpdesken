using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseOverview
{
	public class ExtendedCaseOverview
	{
		public ExtendedCaseOverview(List<ExtendedCaseOverviewValue> values)
		{
			Values = values;
		}
		public List<ExtendedCaseOverviewValue> Values { get; set; }
	}

	public class ExtendedCaseOverviewValue
	{
		public ExtendedCaseOverviewValue(string fieldId, string name, string value)
		{
			FieldId = fieldId;
			Name = name;
			Value = value;
		}
		public string FieldId { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
	}
}
