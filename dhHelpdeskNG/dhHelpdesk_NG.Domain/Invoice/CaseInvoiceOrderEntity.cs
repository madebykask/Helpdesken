namespace DH.Helpdesk.Domain.Invoice
{
    using global::System;
    using global::System.Collections.Generic;

    public class CaseInvoiceOrderEntity : Entity
    {
        public int InvoiceId { get; set; }

        public virtual CaseInvoiceEntity Invoice { get; set; }

        public short Number { get; set; }

        public string DeliveryPeriod { get; set; }

        public string Reference { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<CaseInvoiceArticleEntity> Articles { get; set; } 

        public virtual ICollection<CaseInvoiceOrderFileEntity> Files { get; set; } 
    }
}