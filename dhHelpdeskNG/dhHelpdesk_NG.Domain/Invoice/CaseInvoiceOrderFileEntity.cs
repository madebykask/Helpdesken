namespace DH.Helpdesk.Domain.Invoice
{
    using global::System;

    public class CaseInvoiceOrderFileEntity : Entity
    {
        public int OrderId { get; set; }

        public string FileName { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual CaseInvoiceOrderEntity Order { get; set; }
    }
}