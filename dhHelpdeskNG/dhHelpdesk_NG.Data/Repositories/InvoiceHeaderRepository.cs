using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    #region INVOICEHEADER

    public interface IInvoiceHeaderRepository : IRepository<InvoiceHeader>
    {
    }

    public class InvoiceHeaderRepository : RepositoryBase<InvoiceHeader>, IInvoiceHeaderRepository
    {
        public InvoiceHeaderRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region INVOICEROW

    public interface IInvoiceRowRepository : IRepository<InvoiceRow>
    {
    }

    public class InvoiceRowRepository : RepositoryBase<InvoiceRow>, IInvoiceRowRepository
    {
        public InvoiceRowRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
