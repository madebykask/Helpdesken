namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;
    using System.Xml;
    using System.Xml.Serialization;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public sealed class InvoiceHelper : IInvoiceHelper
    {
        public CaseInvoice[] ToCaseInvoices(
                    string invoices, 
                    CaseOverview caseOverview, 
                    InvoiceArticle[] articles)
        {
            if (string.IsNullOrEmpty(invoices))
            {
                return null;
            }

            var serializer = new JavaScriptSerializer();
            var invoiceData = serializer.Deserialize<CaseInvoiceData>(invoices);

            var now = DateTime.Now;
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
                                    now,
                                    o.Articles
                                    .Select(a => new CaseInvoiceArticle(
                                            a.Id,
                                            a.OrderId,
                                            null,
                                            a.ArticleId,
                                            articles != null ? articles.FirstOrDefault(ar => ar.Id == a.ArticleId) : null,
                                            a.Name,
                                            a.Amount,
                                            a.Ppu,
                                            a.Position,
                                            a.IsInvoiced)).ToArray())).ToArray());
            if (caseOverview != null)
            {
                foreach (var order in invoice.Orders)
                {
                    order.CaseNumber = caseOverview.CaseNumber;
                }                
            }

            return new[] { invoice };
        }

        public XmlDocument ToOutputXml(CaseInvoice[] invoices)
        {
            if (invoices == null || !invoices.Any())
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                var serializer = new XmlSerializer(typeof(CaseInvoice));
                serializer.Serialize(ms, invoices.First());
                ms.Seek(0, SeekOrigin.Begin);
                var document = new XmlDocument();
                document.Load(ms);
                return document;
            }
        }

        public string GetExportFileName()
        {
            return string.Format("{0}_{1}.xml", DateTime.Now.ToShortDateString(), Guid.NewGuid());
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

            public DateTime Date { get; set; }

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