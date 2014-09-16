namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using DH.Helpdesk.Common.ValidationAttributes;

    [Serializable]
    [XmlRoot("SalesLine")]
    public sealed class CaseInvoiceArticle : IXmlSerializable
    {
        public CaseInvoiceArticle(
                int id,      
                int orderId,
                CaseInvoiceOrder order,
                int? articleId, 
                InvoiceArticle article,
                string name, 
                int? amount,
                decimal? ppu,
                short position,
                bool isInvoiced)
        {
            this.OrderId = orderId;
            this.Order = order;
            this.Ppu = ppu;
            this.Article = article;
            this.ArticleId = articleId;
            this.Position = position;
            this.Amount = amount;
            this.IsInvoiced = isInvoiced;
            this.Name = name;
            this.Id = id;
        }        

        public CaseInvoiceArticle()
        {            
        }

        public int Id { get; private set; }

        public int OrderId { get; private set; }

        public CaseInvoiceOrder Order { get; private set; }

        public int? ArticleId { get; private set; }

        public InvoiceArticle Article { get; private set; }

        [NotNull]
        public string Name { get; private set; }

        public int? Amount { get; private set; }

        public decimal? Ppu { get; private set; }

        public short Position { get; private set; }

        public bool IsInvoiced { get; private set; }

        public bool IsNew()
        {
            return this.Id <= 0;
        }

        public void MakeInvoiced()
        {
            this.IsInvoiced = true;
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
            if (this.Article == null)
            {
                writer.WriteElementString("Description", this.Name);
            }

            if (this.Article != null)
            {
                writer.WriteElementString("ItemNo", this.Article.Number);
                if (this.Article.Unit != null)
                {
                    writer.WriteElementString("UnitOfMeasureCode", this.Article.Unit.Name);                    
                }

                var ppu = this.Ppu.HasValue ? this.Ppu.Value : (this.Article.Ppu.HasValue ? this.Article.Ppu.Value : (decimal?)null);
                if (ppu.HasValue)
                {
                    writer.WriteElementString("UnitPrice", ppu.Value.ToString(CultureInfo.InvariantCulture));                    
                }
            }

            if (this.Amount.HasValue)
            {
                writer.WriteElementString("Quantity", this.Amount.Value.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}