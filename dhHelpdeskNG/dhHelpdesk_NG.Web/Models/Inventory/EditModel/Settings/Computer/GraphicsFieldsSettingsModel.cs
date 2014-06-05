namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class GraphicsFieldsSettingsModel
    {
        public GraphicsFieldsSettingsModel()
        {
        }

        public GraphicsFieldsSettingsModel(FieldSettingModel videoCardFieldSettingModel)
        {
            this.VideoCardFieldSettingModel = videoCardFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Video Card")]
        public FieldSettingModel VideoCardFieldSettingModel { get; set; }
    }
}