namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories.Invoice;
    using DH.Helpdesk.Domain.Invoice;

    public sealed class InvoiceArticleService : IInvoiceArticleService
    {
        private readonly IInvoiceArticleUnitRepository invoiceArticleUnitRepository;

        private readonly IInvoiceArticleRepository invoiceArticleRepository;

        private readonly ICaseInvoiceArticleRepository caseInvoiceArticleRepository;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public InvoiceArticleService(
                IInvoiceArticleUnitRepository invoiceArticleUnitRepository, 
                IInvoiceArticleRepository invoiceArticleRepository, 
                ICaseInvoiceArticleRepository caseInvoiceArticleRepository, 
                IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.invoiceArticleUnitRepository = invoiceArticleUnitRepository;
            this.invoiceArticleRepository = invoiceArticleRepository;
            this.caseInvoiceArticleRepository = caseInvoiceArticleRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
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
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var caseInvoicesRep = uow.GetRepository<CaseInvoiceEntity>();
                var caseInvoiceOrdersRep = uow.GetRepository<CaseInvoiceOrderEntity>();
                var caseInvoiceArticlesRep = uow.GetRepository<CaseInvoiceArticleEntity>();
                var caseInvoiceOrderFilesRep = uow.GetRepository<CaseInvoiceOrderFileEntity>();

                var invoices = caseInvoicesRep.GetAll()
                                .Where(i => i.CaseId == caseId)
                                .ToList();

                foreach (var invoice in invoices)
                {
                    var orderIds = invoice.Orders.Select(o => o.Id);
                    caseInvoiceArticlesRep.DeleteWhere(a => orderIds.Contains(a.OrderId));
                    caseInvoiceOrderFilesRep.DeleteWhere(f => orderIds.Contains(f.OrderId));
                    caseInvoiceOrdersRep.DeleteWhere(o => orderIds.Contains(o.Id));
                    caseInvoicesRep.DeleteById(invoice.Id);
                }

                uow.Save();
            }
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