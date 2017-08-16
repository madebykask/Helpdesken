using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.CaseDocument
{
	public enum CaseDocumentParagraphType
	{
		Text = 1,
		TableNumeric = 2,
		Logo = 3,
		TableTwoColumns = 4,
		Footer = 5,
		DraftSymbol = 6,
		Header = 7
	}
}
