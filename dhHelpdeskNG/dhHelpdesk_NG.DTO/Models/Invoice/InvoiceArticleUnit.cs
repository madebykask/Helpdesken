namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InvoiceArticleUnit
    {
        public InvoiceArticleUnit(int id, string name)
        {
            this.Name = name;
            this.Id = id;
        }

        [IsId]
        public int Id { get; private set; }

        [NotNull]
        public string Name { get; private set; }
    }
}