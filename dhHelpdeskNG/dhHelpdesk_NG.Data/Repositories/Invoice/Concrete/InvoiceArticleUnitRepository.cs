namespace DH.Helpdesk.Dal.Repositories.Invoice.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class InvoiceArticleUnitRepository : Repository, IInvoiceArticleUnitRepository
    {
        public InvoiceArticleUnitRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}