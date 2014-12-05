namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings;

    public interface IFieldSettingModelBuilder
    {
        FieldSettingModel MapFieldSetting(FieldSetting setting);
    }
}
