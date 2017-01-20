using System.Collections.Generic;

namespace DH.Helpdesk.Domain
{
    using global::System;

    public class InvoiceHeader : Entity
    {
	    public InvoiceHeader()
	    {
		    InvoiceRows = new List<InvoiceRow>();
	    }

		public decimal VerificationNumberEnd { get; set; }
        public decimal VerificationNumberStart { get; set; }
        public int User_Id { get; set; }
        public string InvoiceFilename { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid InvoiceHeaderGUID { get; set; }

		public User CreatedByUser { get; set; }
		public virtual ICollection<InvoiceRow> InvoiceRows { get; set; }
    }
}
