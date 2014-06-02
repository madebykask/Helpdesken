namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings;

    public interface IFieldSettingBuilder
    {
        FieldSetting MapFieldSetting(FieldSettingModel setting);
    }
}