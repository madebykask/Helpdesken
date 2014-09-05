namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Linq;
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public static class InvoiceHelper
    {
        public static CaseInvoice[] ToCaseInvoices(string invoices)
        {
            if (string.IsNullOrEmpty(invoices))
            {
                return null;
            }

            var serializer = new JavaScriptSerializer();
            var invoiceData = serializer.Deserialize<CaseInvoiceData>(invoices);
            var invoice = new CaseInvoice(
                        invoiceData.Id,
                        invoiceData.CaseId,
                        invoiceData.Orders
                            .Select(o => new CaseInvoiceOrder(
                                    o.Id,
                                    o.InvoiceId,
                                    o.Number,
                                    o.DeliveryPeriod,
                                    o.Reference,
                                    o.Articles
                                    .Select(a => new CaseInvoiceArticle(
                                            a.Id,
                                            a.OrderId,
                                            a.ArticleId,
                                            a.Name,
                                            a.Amount,
                                            a.Ppu,
                                            a.Position,
                                            a.IsInvoiced)).ToArray())).ToArray());

            return new[] { invoice };
        }

        private class CaseInvoiceData
        {
            public CaseInvoiceData()
            {
            }

            public int Id { get; set; }

            public int CaseId { get; set; }

            public CaseInvoiceOrderData[] Orders { get; set; }
        }

        private class CaseInvoiceOrderData
        {
            public CaseInvoiceOrderData()
            {
            }

            public int Id { get; set; }

            public int InvoiceId { get; set; }

            public short Number { get; set; }

            public string DeliveryPeriod { get; set; }

            public string Reference { get; set; }

            public CaseInvoiceArticleData[] Articles { get; set; }
        }

        private class CaseInvoiceArticleData
        {
            public CaseInvoiceArticleData()
            {
            }

            public int Id { get; set; }

            public int OrderId { get; set; }

            public int? ArticleId { get; set; }

            public string Name { get; set; }

            public int? Amount { get; set; }

            public decimal? Ppu { get; set; }

            public short Position { get; set; }

            public bool IsInvoiced { get; set; }
        }
    }    
}