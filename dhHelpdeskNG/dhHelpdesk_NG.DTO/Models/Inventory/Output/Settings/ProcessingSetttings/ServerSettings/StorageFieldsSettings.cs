namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageFieldsSettings
    {
        public StorageFieldsSettings(ProcessingFieldSetting capasityFieldSetting)
        {
            this.CapasityFieldSetting = capasityFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting CapasityFieldSetting { get; set; }
    }
}