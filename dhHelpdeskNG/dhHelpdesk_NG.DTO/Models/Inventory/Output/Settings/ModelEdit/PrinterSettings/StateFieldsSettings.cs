namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(ModelEditFieldSetting createdDateFieldSetting, 
                                   ModelEditFieldSetting changedDateFieldSetting,
                                   ModelEditFieldSetting syncDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncDateFieldSetting = syncDateFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ChangedDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting SyncDateFieldSetting { get; }
    }
}