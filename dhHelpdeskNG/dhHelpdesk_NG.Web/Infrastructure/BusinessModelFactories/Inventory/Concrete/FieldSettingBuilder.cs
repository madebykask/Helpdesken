namespace DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings;

    public class FieldSettingBuilder : IFieldSettingBuilder
    {
        public FieldSetting MapFieldSetting(FieldSettingModel setting)
        {
            var settingModel = new FieldSetting(
                setting.ShowInDetails,
                setting.ShowInList,
                setting.Caption,
                setting.IsRequired);

            return settingModel;
        }
    }
}