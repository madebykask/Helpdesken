namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
