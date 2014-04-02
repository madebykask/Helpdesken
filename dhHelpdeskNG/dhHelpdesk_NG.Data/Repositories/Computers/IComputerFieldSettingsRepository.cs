namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerFieldSettingsRepository : INewRepository
    {
        void Update(ComputerFieldsSettings businessModel);

        ComputerFieldsSettings GetFieldSettingsForEdit(int customerId, int languageId);

        ComputerFieldsSettingsForModelEdit GetFieldSettingsForModelEdit(int customerId, int languageId);

        ComputerFieldsSettingsOverview GetFieldSettingsOverview(int customerId, int languageId);

        ComputerFieldsSettingsOverviewForFilter GetFieldSettingsOverviewForFilter(int customerId, int languageId);
    }
}