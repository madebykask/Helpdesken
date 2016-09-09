namespace DH.Helpdesk.Web.Areas.Admin.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;

    public sealed class InvoiceArticleProductAreaIndexRowModel
    {

        public InvoiceArticleProductAreaIndexRowModel()
        {

        }
        public int InvoiceArticleId { get; set; }

        public string InvoiceArticleName { get; set; }

        public int ProductAreaId { get; set; }

        public string ProductAreaName { get; set; }

        public string InvoiceArticleNumber { get; set; }

    }

    public sealed class InvoiceArticleProductAreaIndexRowsModel
    {
        public InvoiceArticleProductAreaIndexRowsModel(Customer customer)
        {
            Data = new List<InvoiceArticleProductAreaIndexRowModel>();
            Customer = customer;
        }
        public Customer Customer { get; private set; }
        public List<InvoiceArticleProductAreaIndexRowModel> Data { get; set; }
    }

    public sealed class InvoiceArticleProductAreaIndexModel
    {

        public InvoiceArticleProductAreaIndexModel(Customer customer)
        {
            Customer = customer;
            Rows = new InvoiceArticleProductAreaIndexRowsModel(customer);
        }
        
        public Customer Customer { get; private set; }

        public List<InvoiceArticle> InvoiceArticles { get; set; }
        public List<ProductArea> ProductAreas { get; set; }
        public InvoiceArticleProductAreaIndexRowsModel Rows { get; set; }
    }


}