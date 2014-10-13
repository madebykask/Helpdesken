namespace DH.Helpdesk.Mobile.Infrastructure.BusinessModelFactories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings;

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