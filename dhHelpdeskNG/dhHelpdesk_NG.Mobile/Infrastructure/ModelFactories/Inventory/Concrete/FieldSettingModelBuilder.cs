namespace DH.Helpdesk.Mobile.Infrastructure.ModelFactories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Mobile.Models.Inventory.EditModel.Settings;

    public class FieldSettingModelBuilder : IFieldSettingModelBuilder
    {
        public FieldSettingModel MapFieldSetting(FieldSetting setting)
        {
            var settingModel = new FieldSettingModel(
                setting.ShowInDetails,
                setting.ShowInList,
                setting.Caption,
                setting.IsRequired);

            return settingModel;
        }
    }
}