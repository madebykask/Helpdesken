namespace DH.Helpdesk.Web.Areas.Admin.Models.Invoice
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public class InvoiceArticleProductAreaInputViewModel
    {
        public Customer Customer { get; set; }

        public List<InvoiceArticle> Articles { get; set; }

        public List<ProductArea> ProductAreas { get; set; }
    }
}