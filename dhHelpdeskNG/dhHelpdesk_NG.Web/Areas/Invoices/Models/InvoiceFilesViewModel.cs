using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceFilesViewModel : BaseIndexModel
	{
		public override IndexModelType Type => IndexModelType.InvoiceFiles;

		public Dictionary<string, Dictionary<string, List<InvoiceFileViewModel>>> Files { get; set; }
	}
}