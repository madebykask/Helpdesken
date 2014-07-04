namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Repositories.Invoice;

    public sealed class InvoiceArticleService : IInvoiceArticleService
    {
        private readonly IInvoiceArticleUnitRepository invoiceArticleUnitRepository;

        private readonly IInvoiceArticleRepository invoiceArticleRepository;

        private readonly ICaseInvoiceArticleRepository caseInvoiceArticleRepository;

        public InvoiceArticleService(
                IInvoiceArticleUnitRepository invoiceArticleUnitRepository, 
                IInvoiceArticleRepository invoiceArticleRepository, 
                ICaseInvoiceArticleRepository caseInvoiceArticleRepository)
        {
            this.invoiceArticleUnitRepository = invoiceArticleUnitRepository;
            this.invoiceArticleRepository = invoiceArticleRepository;
            this.caseInvoiceArticleRepository = caseInvoiceArticleRepository;
        }

        public InvoiceArticleUnit[] GetUnits(int customerId)
        {
            return this.invoiceArticleUnitRepository.GetUnits(customerId);
        }

        public InvoiceArticle[] GetArticles(int customerId, int productAreaId)
        {
            return this.invoiceArticleRepository.GetArticles(customerId, productAreaId);
        }

        public CaseInvoiceArticle[] GetCaseArticles(int caseId)
        {
            return this.caseInvoiceArticleRepository.GetCaseArticles(caseId);
        }

        public void SaveCaseArticles(int caseId,  CaseInvoiceArticle[] articles)
        {
            this.caseInvoiceArticleRepository.SaveCaseArticles(caseId, articles);
        }

        public void DeleteCaseArticles(int caseId)
        {
            this.caseInvoiceArticleRepository.DeleteCaseArticles(caseId);
        }
    }
}