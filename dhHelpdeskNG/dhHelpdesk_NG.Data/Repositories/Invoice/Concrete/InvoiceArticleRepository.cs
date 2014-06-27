namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InvoiceArticleRepository : Repository, IInvoiceArticleRepository
    {
        public InvoiceArticleRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}