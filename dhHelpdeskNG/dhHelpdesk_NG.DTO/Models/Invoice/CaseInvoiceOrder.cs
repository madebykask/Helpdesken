namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    public sealed class CaseInvoiceOrder
    {
        public CaseInvoiceOrder(
                int id, 
                int invoiceId,
                CaseInvoice invoice, 
                short number, 
                string deliveryPeriod, 
                string reference,
                CaseInvoiceArticle[] articles)
        {
            this.Reference = reference;
            this.Articles = articles;
            this.InvoiceId = invoiceId;
            this.DeliveryPeriod = deliveryPeriod;
            this.Number = number;
            this.Invoice = invoice;
            this.Id = id;
        }

        public CaseInvoiceOrder(
                int id, 
                int invoiceId,
                short number, 
                string deliveryPeriod, 
                string reference,
                CaseInvoiceArticle[] articles) :
                this(id, invoiceId, null, number, deliveryPeriod, reference, articles)
        {
        }

        public int Id { get; private set; }

        public int InvoiceId { get; private set; }

        public CaseInvoice Invoice { get; private set; }

        public short Number { get; private set; }

        public string DeliveryPeriod { get; private set; }

        public string Reference { get; private set; }

        public CaseInvoiceArticle[] Articles { get; private set; } 
    }
}