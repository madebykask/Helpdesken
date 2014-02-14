namespace DH.Helpdesk.Dal.Repositories.Printers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class PrinterFieldSettingsRepository : Repository, IPrinterFieldSettingsRepository
    {
        public PrinterFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}