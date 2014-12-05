namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings;

    public interface IFieldSettingBuilder
    {
        FieldSetting MapFieldSetting(FieldSettingModel setting);
    }
}