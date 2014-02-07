namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region PRINTER

    public interface IPrinterRepository : IRepository<Printer>
    {
    }

    public class PrinterRepository : RepositoryBase<Printer>, IPrinterRepository
    {
        public PrinterRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region PRINTERFIELDSETTINGS

    public interface IPrinterFieldSettingsRepository : IRepository<PrinterFieldSettings>
    {
    }

    public class PrinterFieldSettingsRepository : RepositoryBase<PrinterFieldSettings>, IPrinterFieldSettingsRepository
    {
        public PrinterFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
