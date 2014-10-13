namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

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

        public InvoiceArticle[] GetArticles(int customerId)
        {
            return this.invoiceArticleRepository.GetArticles(customerId);
        }

        public CaseInvoice[] GetCaseInvoices(int caseId)
        {
            return this.caseInvoiceArticleRepository.GetCaseInvoices(caseId);
        }

        public void SaveCaseInvoices(IEnumerable<CaseInvoice> invoices, int caseId)
        {
            this.caseInvoiceArticleRepository.SaveCaseInvoices(invoices, caseId);
        }

        public void DeleteCaseInvoices(int caseId)
        {
            this.caseInvoiceArticleRepository.DeleteCaseInvoices(caseId);
        }

        public int SaveArticle(InvoiceArticle article)
        {
            return this.invoiceArticleRepository.SaveArticle(article);
        }

        public int SaveUnit(InvoiceArticleUnit unit)
        {
            return this.invoiceArticleUnitRepository.SaveUnit(unit);
        }
    }
}