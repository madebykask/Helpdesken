﻿namespace DH.Helpdesk.BusinessData.Models.Invoice
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
                decimal? amount,
                decimal? ppu,
                short position,
                int? creditedForArticle_Id,
                int? textForArticle_Id)
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
            this.CreditedForArticle_Id = creditedForArticle_Id;
            this.TextForArticle_Id = textForArticle_Id;
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

        public decimal? Amount { get; private set; }

        public decimal? Ppu { get; private set; }

        public short Position { get; private set; }

        public int? CreditedForArticle_Id { get; private set; }

        public int? TextForArticle_Id { get; set; }

        public bool IsNew()
        {
            return this.Id <= 0;
        }

    }
}