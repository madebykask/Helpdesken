namespace DH.Helpdesk.Domain.Invoice
{
    public class CaseInvoiceArticleEntity : Entity
    {
        public int CaseId { get; set; }

        public virtual Case Case { get; set; }

        public int? Number { get; set; }

        public string Name { get; set; }

        public int? Amount { get; set; }

        public int? UnitId { get; set; }

        public virtual InvoiceArticleUnitEntity Unit { get; set; }

        public decimal? Ppu { get; set; }

        public short Position { get; set; }

        public bool IsInvoiced { get; set; }
    }
}