using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case
{
	public class ExtendedCaseFormFieldTranslationModel
	{
		public string FieldId { get; set; }
		public string Text { get; set; }
		public int LanguageId { get; set; }
	}
}
