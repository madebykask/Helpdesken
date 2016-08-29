namespace DH.Helpdesk.Web.Areas.Admin.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;
    using System.Collections.Generic;

    public sealed class InvoiceArticleProductAreaModel
    {

        public InvoiceArticleProductAreaModel()
        {

        }
        public int InvoiceArticleId { get; set; }

        public string InvoiceArticleName { get; set; }

        public int ProductAreaId { get; set; }

        public string ProductAreaName { get; set; }

        public string InvoiceArticleNumber { get; set; }

    }

    public sealed class InvoiceArticleProductAreaIndexModel:List<InvoiceArticleProductAreaModel>
    {

        public InvoiceArticleProductAreaIndexModel(Customer customer)
        {
            Customer = customer;
        }
        
        public Customer Customer { get; private set; }

        public List<InvoiceArticle> InvoiceArticles { get; set; }
        public List<ProductArea> ProductAreas { get; set; }
    }


}