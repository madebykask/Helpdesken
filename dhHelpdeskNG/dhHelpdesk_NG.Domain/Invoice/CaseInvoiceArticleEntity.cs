namespace DH.Helpdesk.Domain.Invoice
{
    public class CaseInvoiceArticleEntity : Entity
    {
        public int OrderId { get; set; }

        public virtual CaseInvoiceOrderEntity Order { get; set; }

        public int? ArticleId { get; set; }

        public virtual InvoiceArticleEntity Article { get; set; }

        public string Name { get; set; }

        public int? Amount { get; set; }

        public decimal? Ppu { get; set; }

        public short Position { get; set; }

        //public bool IsInvoiced { get; set; }
    }
}