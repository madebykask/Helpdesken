namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProcessorFieldsSettings
    {
        public ProcessorFieldsSettings(ProcessingFieldSetting proccesorFieldSettings)
        {
            this.ProccesorFieldSetting = proccesorFieldSettings;
        }

        [NotNull]
        public ProcessingFieldSetting ProccesorFieldSetting { get; set; }
    }
}