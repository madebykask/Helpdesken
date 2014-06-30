namespace DH.Helpdesk.Web.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseInvoiceArticlesModel
    {
        public CaseInvoiceArticlesModel(
            CaseInvoiceArticle[] caseArticles)
        {
            this.CaseArticles = caseArticles;
        }

        [NotNull]
        public CaseInvoiceArticle[] CaseArticles { get; private set; }
    }
}