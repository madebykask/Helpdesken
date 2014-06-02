namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings;

    public interface IFieldSettingModelBuilder
    {
        FieldSettingModel MapFieldSetting(FieldSetting setting);
    }
}
