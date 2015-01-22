namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot("SalesHeader")]
    public sealed class CaseInvoiceOrder : IXmlSerializable
    {
        public CaseInvoiceOrder(
                int id, 
                int invoiceId,
                CaseInvoice invoice, 
                short number, 
                string deliveryPeriod, 
                string reference,
                DateTime date,
                CaseInvoiceArticle[] articles,
                CaseInvoiceOrderFile[] files)
        {
            this.Reference = reference;
            this.Articles = articles;
            this.InvoiceId = invoiceId;
            this.DeliveryPeriod = deliveryPeriod;
            this.Number = number;
            this.Invoice = invoice;
            this.Id = id;
            this.Date = date;
            this.Files = files;
        }

        public CaseInvoiceOrder(
                int id, 
                int invoiceId,
                short number, 
                string deliveryPeriod, 
                string reference,
                DateTime date,
                CaseInvoiceArticle[] articles,
                CaseInvoiceOrderFile[] files) :
                this(id, invoiceId, null, number, deliveryPeriod, reference, date, articles, files)
        {
        }

        public CaseInvoiceOrder()
        {            
        }

        public int Id { get; private set; }

        public int InvoiceId { get; private set; }

        public CaseInvoice Invoice { get; private set; }

        public short Number { get; private set; }

        public string DeliveryPeriod { get; private set; }

        public string Reference { get; private set; }

        public DateTime Date { get; private set; }

        public CaseInvoiceArticle[] Articles { get; private set; }

        public CaseInvoiceOrderFile[] Files { get; private set; }

        public decimal? CaseNumber { get; set; }

        public void DoInvoice()
        {
            foreach (var article in this.Articles)
            {
                article.DoInvoice();
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("OrderDate", this.Date.ToShortDateString());
            writer.WriteElementString("OurReferenceNo", this.Reference);
            if (this.CaseNumber.HasValue)
            {
                writer.WriteElementString("CaseNo", this.CaseNumber.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (this.Articles != null)
            {
                var serializer = new XmlSerializer(typeof(CaseInvoiceArticle));
                foreach (var article in this.Articles)
                {
                    serializer.Serialize(writer, article);                    
                }
            }

            if (this.Files != null)
            {
                var serializer = new XmlSerializer(typeof(CaseInvoiceOrderFile));
                foreach (var file in this.Files)
                {
                    serializer.Serialize(writer, file);
                }
            }
        }
    }
}