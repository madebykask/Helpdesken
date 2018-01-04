namespace DH.Helpdesk.Dal.Repositories.Printers
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.PrinterFieldSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IPrinterFieldSettingsRepository : INewRepository
    {
        void Update(PrinterFieldsSettings businessModel);

        PrinterFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId);

        PrinterFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId, bool isReadonly = false);

        PrinterFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId);

        PrinterFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int customerId, int languageId);

        PrinterFieldsSettingsProcessing GetFieldSettingsProcessing(int customerId);
    }
}