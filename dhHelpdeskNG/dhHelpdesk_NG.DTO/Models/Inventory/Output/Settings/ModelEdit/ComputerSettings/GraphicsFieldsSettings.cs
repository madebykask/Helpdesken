namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GraphicsFieldsSettings
    {
        public GraphicsFieldsSettings(ModelEditFieldSetting videoCardFieldSetting)
        {
            this.VideoCardFieldSetting = videoCardFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting VideoCardFieldSetting { get; set; }
    }
}