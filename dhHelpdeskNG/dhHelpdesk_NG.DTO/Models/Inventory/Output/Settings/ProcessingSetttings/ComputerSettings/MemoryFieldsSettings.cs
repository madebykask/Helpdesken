namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class MemoryFieldsSettings
    {
        public MemoryFieldsSettings(ProcessingFieldSetting ramFieldSetting)
        {
            this.RAMFieldSetting = ramFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting RAMFieldSetting { get; set; }
    }
}