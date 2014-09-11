namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InvoiceArticleUnit
    {
        public InvoiceArticleUnit(
                    int id, 
                    string name, 
                    int customerId)
        {
            this.CustomerId = customerId;
            this.Name = name;
            this.Id = id;
        }

        public InvoiceArticleUnit(string name)
        {
            this.Name = name;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        [IsId]
        public int CustomerId { get; set; }
    }
}