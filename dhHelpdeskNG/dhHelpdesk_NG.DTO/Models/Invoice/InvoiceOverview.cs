using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain.Invoice;

namespace DH.Helpdesk.BusinessData.Models.Invoice
{
	public class InvoiceOverview
	{
		public InvoiceOverview()
		{
			LogInvoices = new List<CaseLog>();
			ExternalInvoices = new List<ExternalInvoice.ExternalInvoice>();
		}

		public int CaseId { get; set; }
		public decimal CaseNumber { get; set; }
		public string Caption { get; set; }
		public string Category { get; set; }
		public DateTime? FinishingDate { get; set; }
		public string Department { get; set; }
		
		public int WorkingHourRate { get; set; }
		public int OvertimeHourRate { get; set; }        
		public List<CaseLog> LogInvoices { get; set; }
		public List<ExternalInvoice.ExternalInvoice> ExternalInvoices { get; set; }
        public InvoiceRowStatistics Statistics { get; set; }
    }
}
