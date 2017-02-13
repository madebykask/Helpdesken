using System.Collections.Generic;
using DH.Helpdesk.Domain.Invoice;

namespace DH.Helpdesk.Domain
{
    using global::System;

    public class InvoiceRow : Entity
    {
        public InvoiceRow()
        {
			CaseInvoiceRows = new List<CaseInvoiceRow>();
			Logs = new List<Log>();
        }

        public int? Case_Id { get; set; }
        public int? InvoiceHeader_Id { get; set; }
        public string Equipment_Account1Debit { get; set; }
        public string Equipment_Account2Debit { get; set; }
        public string Equipment_Account3Debit { get; set; }
        public string Equipment_Account4Debit { get; set; }
        public string Equipment_Account5Debit { get; set; }
        public string Equipment_Account6Debit { get; set; }
        public string Equipment_Account7Debit { get; set; }
        public string Equipment_Account8Debit { get; set; }
        public string Equipment_Account1Kredit { get; set; }
        public string Equipment_Account2Kredit { get; set; }
        public string Equipment_Account3Kredit { get; set; }
        public string Equipment_Account4Kredit { get; set; }
        public string Equipment_Account5Kredit { get; set; }
        public string Equipment_Account6Kredit { get; set; }
        public string Equipment_Account7Kredit { get; set; }
        public string Equipment_Account8Kredit { get; set; }
        public string Equipment_VerificationText { get; set; }
        public string Time_Account1Debit { get; set; }
        public string Time_Account2Debit { get; set; }
        public string Time_Account3Debit { get; set; }
        public string Time_Account4Debit { get; set; }
        public string Time_Account5Debit { get; set; }
        public string Time_Account6Debit { get; set; }
        public string Time_Account7Debit { get; set; }
        public string Time_Account8Debit { get; set; }
        public string Time_Account1Kredit { get; set; }
        public string Time_Account2Kredit { get; set; }
        public string Time_Account3Kredit { get; set; }
        public string Time_Account4Kredit { get; set; }
        public string Time_Account5Kredit { get; set; }
        public string Time_Account6Kredit { get; set; }
        public string Time_Account7Kredit { get; set; }
        public string Time_Account8Kredit { get; set; }
        public string Time_VerificationText { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
	    public InvoiceStatus Status { get; set; }
		public int? CreatedByUser_Id { get; set; }

		public virtual User CreatedByUser { get; set; }
		public virtual Case Case { get; set; }
        public virtual InvoiceHeader InvoiceHeader { get; set; }
		public virtual IList<CaseInvoiceRow> CaseInvoiceRows { get; set; }
		public virtual IList<Log> Logs { get; set; }
	}
}
