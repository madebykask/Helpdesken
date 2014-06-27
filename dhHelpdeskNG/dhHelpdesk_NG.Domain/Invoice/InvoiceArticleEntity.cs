namespace DH.Helpdesk.Domain.Invoice
{
    public class InvoiceArticleEntity : Entity
    {
        public int? ParentId { get; set; }

        public virtual InvoiceArticleEntity Parent { get; set; }

        public int Number { get; set; }

        public string Name { get; set; }

        public int UnitId { get; set; }

        public virtual InvoiceArticleUnitEntity Unit { get; set; }

        public decimal Ppu { get; set; }

        public int ProductAreaId { get; set; }

        public virtual ProductArea ProductArea { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}