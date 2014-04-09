namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GraphicsFieldsSettingsModel
    {
        public GraphicsFieldsSettingsModel(FieldSettingModel videoCardFieldSettingModel)
        {
            this.VideoCardFieldSettingModel = videoCardFieldSettingModel;
        }

        [NotNull]
        public FieldSettingModel VideoCardFieldSettingModel { get; set; }
    }
}