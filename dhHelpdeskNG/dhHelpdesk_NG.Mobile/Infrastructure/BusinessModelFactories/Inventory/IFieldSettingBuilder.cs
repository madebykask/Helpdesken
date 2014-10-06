namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings;

    public interface IFieldSettingBuilder
    {
        FieldSetting MapFieldSetting(FieldSettingModel setting);
    }
}