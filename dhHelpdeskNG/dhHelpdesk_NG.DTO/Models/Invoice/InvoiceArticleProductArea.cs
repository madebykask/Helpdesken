namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class InvoiceArticleProductArea
    {
        public InvoiceArticleProductArea(
                int invoiceArticle_Id,
                int productArea_Id)
        {
            this.InvoiceArticle_Id = invoiceArticle_Id;
            this.ProductArea_Id = productArea_Id;
        }

        public InvoiceArticleProductArea()
        {
        }

        public int InvoiceArticle_Id { get; private set; }

        public int ProductArea_Id { get; private set; }
    }
}