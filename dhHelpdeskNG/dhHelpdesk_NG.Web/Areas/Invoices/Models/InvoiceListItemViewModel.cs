using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Web.Models.Invoice;
using DH.Helpdesk.BusinessData.Models.Invoice;

namespace DH.Helpdesk.Web.Areas.Invoices.Models
{
	public class InvoiceListItemViewModel
	{
		public InvoiceListItemViewModel()
		{
			LogInvoices = new List<LogInvoiceItemViewModel>();
			ExternalInvoices = new List<ExternalInvoiceModel>();
		}

		public int CaseId { get; set; }
		public string CaseNumber { get; set; }
		public string Caption { get; set; }
		public string Category { get; set; }
		public DateTime? FinishingDate { get; set; }
		public string Department { get; set; }
		
		public List<LogInvoiceItemViewModel> LogInvoices { get; set; }
		public List<ExternalInvoiceModel> ExternalInvoices { get; set; }

        public InvoiceRowStatistics Statistics { get; set; }

    }
}