using System.Collections.Generic;
namespace DH.Helpdesk.Domain.Invoice
{
    public class InvoiceArticleEntity : Entity
    {
        public int? ParentId { get; set; }

        public virtual InvoiceArticleEntity Parent { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string NameEng { get; set; }

        public string Description { get; set; }

        public int? UnitId { get; set; }

        public virtual InvoiceArticleUnitEntity Unit { get; set; }

        public decimal? Ppu { get; set; }

        //public int ProductAreaId { get; set; } old deprecated not used

        public virtual ICollection<ProductArea> ProductAreas { get; set; }

        public bool? TextDemand { get; set; }

        public bool? Blocked { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}