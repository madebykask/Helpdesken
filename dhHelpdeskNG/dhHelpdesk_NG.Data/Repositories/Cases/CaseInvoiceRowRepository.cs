namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ICaseInvoiceRowRepository : IRepository<CaseInvoiceRow>
    {
    }

    public class CaseInvoiceRowRepository : RepositoryBase<CaseInvoiceRow>, ICaseInvoiceRowRepository
    {
        public CaseInvoiceRowRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

}
