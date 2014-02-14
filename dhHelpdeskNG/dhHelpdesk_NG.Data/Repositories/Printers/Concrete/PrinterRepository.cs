namespace DH.Helpdesk.Dal.Repositories.Printers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class PrinterRepository : Repository, IPrinterRepository
    {
        public PrinterRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
