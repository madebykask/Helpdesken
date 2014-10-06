namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings;

    public interface IFieldSettingModelBuilder
    {
        FieldSettingModel MapFieldSetting(FieldSetting setting);
    }
}
