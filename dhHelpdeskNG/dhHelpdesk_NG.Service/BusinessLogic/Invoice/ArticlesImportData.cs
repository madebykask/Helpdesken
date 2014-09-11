namespace DH.Helpdesk.Services.BusinessLogic.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;

    public sealed class ArticlesImportData
    {
        public ArticlesImportData(
            ProductAreaOverview[] productAreas, 
            InvoiceArticle[] articles, 
            InvoiceArticleUnit[] units)
        {
            this.Units = units;
            this.Articles = articles;
            this.ProductAreas = productAreas;
        }

        public ProductAreaOverview[] ProductAreas { get; private set; }

        public InvoiceArticle[] Articles { get; private set; }

        public InvoiceArticleUnit[] Units { get; private set; }
    }
}