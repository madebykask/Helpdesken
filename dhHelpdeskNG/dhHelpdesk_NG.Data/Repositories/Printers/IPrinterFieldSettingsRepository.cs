namespace DH.Helpdesk.Dal.Repositories.Printers
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IPrinterFieldSettingsRepository : INewRepository
    {
        void Update(PrinterFieldsSettings businessModel);

        PrinterFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId);

        PrinterFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId);

        PrinterFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId);
    }
}