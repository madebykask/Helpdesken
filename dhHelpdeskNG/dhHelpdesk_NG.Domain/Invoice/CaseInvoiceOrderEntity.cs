namespace DH.Helpdesk.Domain.Invoice
{
    using global::System;
    using global::System.Collections.Generic;

    public class CaseInvoiceOrderEntity : Entity
    {
        public int InvoiceId { get; set; }

        public virtual CaseInvoiceEntity Invoice { get; set; }

        public short Number { get; set; }

        public DateTime DeliveryPeriod { get; set; }

        public virtual ICollection<CaseInvoiceArticleEntity> Articles { get; set; } 
    }
}