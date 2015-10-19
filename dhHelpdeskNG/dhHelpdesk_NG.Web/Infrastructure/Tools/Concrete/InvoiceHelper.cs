namespace DH.Helpdesk.Web.Infrastructure.Tools.Concrete
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
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

            var now = DateTime.UtcNow;
            var invoice = new CaseInvoice(
                        invoiceData.Id,
                        invoiceData.CaseId,
                        invoiceData.Orders
                            .Select(o => new CaseInvoiceOrder(
                                    o.Id,
                                    o.InvoiceId,
                                    o.Number,
                                    o.DeliveryPeriod,
                                    o.InvoicedDate,
                                    o.InvoicedByUserId,
                                    o.Reference,
                                    now,
                                    o.ReportedBy,
                                    o.Persons_Name,
                                    o.Persons_Phone,
                                    o.Persons_Cellphone,
                                    o.Region_Id,
                                    o.Department_Id,
                                    o.OU_Id,
                                    o.Place,
                                    o.UserCode,
                                    o.CostCentre,
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
                                            a.IsInvoiced)).ToArray(),
                                            o.Files.Select(f => new CaseInvoiceOrderFile(HttpUtility.UrlDecode(f.FileName))).ToArray())).ToArray());
            if (caseOverview != null)
            {
                foreach (var order in invoice.Orders)
                {
                    order.CaseNumber = caseOverview.CaseNumber;
                }                
            }

            foreach (var order in invoice.Orders)
            {
                if (order.InvoicedByUserId != null)
                {
                    if (order.InvoicedByUserId != 0)
                    {
                        foreach (var article in order.Articles)
                        {
                            article.DoInvoice();
                        }
                    }
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

        

        public XmlDocument OrderToOutputXML(CaseInvoiceOrder order)
        {
            if (order == null)
            {
                return null;
            }
            var xml = "";

            xml += OrderXMLHeader();
            xml += "<SalesDoc>";
            xml += "<SalesHeader>";
            xml += XMLRow("DocType", "valueplaceholder");
            xml += XMLRow("SellToCustomerNo", "valueplaceholder");
            xml += XMLRow("OrderDate", order.InvoiceDate.Value.ToShortDateString());
            xml += XMLRow("OurReferenceName", "PLACEHOLDERVALUE");
            xml += XMLRow("YourReferenceName", order.CostCentre + "/" + order.Persons_Name);
            xml += XMLRow("OrderNo", "FA" + order.CaseNumber + "-" + order.Number);
            xml += XMLRow("CurrencyCode", "SEK");
            //JOBNO <JobNo />??

            foreach (var article in order.Articles)
            {
                xml += "<SalesLine>";
                xml += XMLRow("ItemNo", article.Article.Number);
                xml += XMLRow("Description", article.Article.Description);
                xml += XMLRow("Quantity", article.Amount.ToString());
                xml += XMLRow("UnitOfMeasureCode", article.Article.Unit.Name);
                var articlePrice = article.Ppu;
                if (articlePrice == null)
	            {
		             articlePrice = article.Article.Ppu;
	            }
                xml += XMLRow("UnitPrice", articlePrice.ToString());
                xml += "</SalesLine>";
            }

            xml += "</SalesHeader>";
            xml += "</SalesDoc>";




            using (var ms = new MemoryStream())
            {
                //var serializer = new XmlSerializer(typeof(CaseInvoiceOrder));
                //serializer.Serialize(ms, order);
                //ms.Seek(0, SeekOrigin.Begin);
                //var document = new XmlDocument();
                //document.Load(ms);
                //return document;


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                return xmlDoc;
            }
        }

        public string GetExportFileName()
        {
            return string.Format("{0}_{1}.xml", DateTime.Now.ToShortDateString(), Guid.NewGuid());
        }

        private string XMLRow(string tag, string value)
        {
            var NewXMLRow = "";
            NewXMLRow = "<" + tag + ">" + value + "</" + tag + ">";

            return NewXMLRow;
        }

        public string OrderXMLHeader()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-16\" standalone=\"no\"?>";
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

            public DateTime? InvoicedDate { get; set; }

            public int? InvoicedByUserId { get; set; }

            public string Reference { get; set; }

            public DateTime Date { get; set; }

            public string ReportedBy { get; set; }

            public string Persons_Name { get; set; }

            public string Persons_Phone { get; set; }

            public string Persons_Cellphone { get; set; }

            public int? Region_Id { get; set; }

            public int? Department_Id { get; set; }

            public int? OU_Id { get; set; }

            public string Place { get; set; }

            public string UserCode { get; set; }

            public string CostCentre { get; set; }

            public CaseInvoiceArticleData[] Articles { get; set; }

            public CaseInvoiceOrderFileData[] Files { get; set; }
        }

        private class CaseInvoiceOrderFileData
        {
            public CaseInvoiceOrderFileData()
            {                
            }

            public string FileName { get; set; }
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