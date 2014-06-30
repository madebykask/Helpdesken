namespace DH.Helpdesk.Domain.Invoice
{
    public class InvoiceArticleUnitEntity : Entity
    {
        public string Name { get; set; }

        public int CustomerId { get; set; }
    }
}