namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Customer.Input;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InvoiceArticle
    {
        public InvoiceArticle(
                    int id, 
                    int? parentId, 
                    InvoiceArticle parent, 
                    int number, 
                    string name, 
                    int? unitId, 
                    InvoiceArticleUnit unit, 
                    decimal? ppu, 
                    int productAreaId, 
                    ProductAreaOverview productArea, 
                    int customerId, 
                    CustomerOverview customer)
        {
            this.Customer = customer;
            this.CustomerId = customerId;
            this.ProductArea = productArea;
            this.ProductAreaId = productAreaId;
            this.Ppu = ppu;
            this.Unit = unit;
            this.UnitId = unitId;
            this.Name = name;
            this.Number = number;
            this.Parent = parent;
            this.ParentId = parentId;
            this.Id = id;
        }

        [IsId]
        public int Id { get; private set; }

        public int? ParentId { get; private set; }

        public InvoiceArticle Parent { get; private set; }

        public int Number { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        public int? UnitId { get; private set; }

        public InvoiceArticleUnit Unit { get; private set; }

        public decimal? Ppu { get; private set; }

        [IsId]
        public int ProductAreaId { get; private set; }

        [NotNull]
        public ProductAreaOverview ProductArea { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        [NotNull]
        public CustomerOverview Customer { get; private set; }
    }
}