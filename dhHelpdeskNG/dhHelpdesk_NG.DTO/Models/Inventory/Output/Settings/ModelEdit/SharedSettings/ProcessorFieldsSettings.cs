namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProcessorFieldsSettings
    {
        public ProcessorFieldsSettings(ModelEditFieldSetting proccesorFieldSettings)
        {
            this.ProccesorFieldSetting = proccesorFieldSettings;
        }

        [NotNull]
        public ModelEditFieldSetting ProccesorFieldSetting { get; set; }
    }
}