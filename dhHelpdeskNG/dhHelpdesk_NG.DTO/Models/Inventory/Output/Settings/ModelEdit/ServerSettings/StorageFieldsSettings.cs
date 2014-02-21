namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StorageFieldsSettings
    {
        public StorageFieldsSettings(ModelEditFieldSetting capasityFieldSetting)
        {
            this.CapasityFieldSetting = capasityFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting CapasityFieldSetting { get; set; }
    }
}