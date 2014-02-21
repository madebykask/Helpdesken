namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(ModelEditFieldSetting createdDateFieldSetting, ModelEditFieldSetting changedDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ChangedDateFieldSetting { get; set; }
    }
}