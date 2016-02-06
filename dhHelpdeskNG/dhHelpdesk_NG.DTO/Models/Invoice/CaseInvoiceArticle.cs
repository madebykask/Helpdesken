namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseInvoiceArticle
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
                short position)
        {
            this.OrderId = orderId;
            this.Order = order;
            this.Ppu = ppu;
            this.Article = article;
            this.ArticleId = articleId;
            this.Position = position;
            this.Amount = amount;
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

        public bool IsNew()
        {
            return this.Id <= 0;
        }

    }
}