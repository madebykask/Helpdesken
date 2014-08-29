namespace DH.Helpdesk.Domain.Invoice
{
    using global::System.Collections.Generic;

    public class CaseInvoiceEntity : Entity
    {
        public int CaseId { get; set; }

        public virtual Case Case { get; set; }

        public virtual ICollection<CaseInvoiceOrderEntity> Orders { get; set; } 
    }
}