namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GraphicsFieldsSettings
    {
        public GraphicsFieldsSettings(ProcessingFieldSetting videoCardFieldSetting)
        {
            this.VideoCardFieldSetting = videoCardFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting VideoCardFieldSetting { get; set; }
    }
}