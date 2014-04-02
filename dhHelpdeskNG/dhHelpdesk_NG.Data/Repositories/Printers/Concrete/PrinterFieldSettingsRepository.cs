namespace DH.Helpdesk.Dal.Repositories.Printers.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class PrinterFieldSettingsRepository : Repository<Domain.Printers.Printer>, IPrinterFieldSettingsRepository
    {
        public PrinterFieldSettingsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Update(PrinterFieldsSettings businessModel)
        {
            throw new System.NotImplementedException();
        }

        public PrinterFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }

        public PrinterFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }

        public PrinterFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId)
        {
            throw new System.NotImplementedException();
        }
    }
}