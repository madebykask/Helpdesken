using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
