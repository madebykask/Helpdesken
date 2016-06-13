namespace DH.Helpdesk.Domain.Invoice
{
    public class CaseInvoiceArticleEntity : Entity
    {
        public int OrderId { get; set; }

        public virtual CaseInvoiceOrderEntity Order { get; set; }

        public int? ArticleId { get; set; }

        public virtual InvoiceArticleEntity Article { get; set; }

        public string Name { get; set; }

        public decimal? Amount { get; set; }

        public decimal? Ppu { get; set; }

        public short Position { get; set; }

        public int? CreditedForArticle_Id { get; set; }

        public int? TextForArticle_Id { get; set; }
        
    }
}