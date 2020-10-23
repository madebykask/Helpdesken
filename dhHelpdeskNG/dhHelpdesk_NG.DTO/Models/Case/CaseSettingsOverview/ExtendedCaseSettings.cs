using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseSettingsOverview
{
	public sealed class ExtendedCaseSettings
	{
		public List<ExtendedCaseSection> Sections { get; set; }
		public List<ExtendedCaseField> Fields { get; set; }
    }

	public sealed class ExtendedCaseField
	{
		public string FieldId { get; set; }
		public string Name { get; set; }
	}

    public sealed class ExtendedCaseSection
    {
        public string SectionId { get; set; }
        public string Name { get; set; }
    }
}
