using System.Collections.Generic;

namespace DH.Helpdesk.Services.BusinessLogic.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;

    public sealed class ArticlesImportData
    {
        public ArticlesImportData(
            List<InvoiceArticle> articles,
            List<InvoiceArticleUnit> units)
        {
            Units = units;
            Articles = articles;
            Errors = new List<string>();
        }

        public List<InvoiceArticle> Articles { get; private set; }

        public List<InvoiceArticleUnit> Units { get; private set; }

        public List<string> Errors { get; set; }
    }
}