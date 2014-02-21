namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GraphicsFieldsSettings
    {
        public GraphicsFieldsSettings(FieldSettingOverview videoCardFieldSetting)
        {
            this.VideoCardFieldSetting = videoCardFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview VideoCardFieldSetting { get; set; }
    }
}