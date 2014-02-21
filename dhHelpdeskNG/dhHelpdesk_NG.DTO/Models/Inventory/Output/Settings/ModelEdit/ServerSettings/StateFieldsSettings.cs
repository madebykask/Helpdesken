namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(ModelEditFieldSetting createdDateFieldSetting, ModelEditFieldSetting changedDateFieldSetting, ModelEditFieldSetting syncChangeDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncChangeDateFieldSetting = syncChangeDateFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting SyncChangeDateFieldSetting { get; set; }
    }
}