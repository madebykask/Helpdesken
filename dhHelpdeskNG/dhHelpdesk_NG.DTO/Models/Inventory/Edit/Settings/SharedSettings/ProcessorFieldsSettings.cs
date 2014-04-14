namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProcessorFieldsSettings
    {
        public ProcessorFieldsSettings(FieldSetting proccesorFieldSettings)
        {
            this.ProccesorFieldSetting = proccesorFieldSettings;
        }

        [NotNull]
        public FieldSetting ProccesorFieldSetting { get; set; }
    }
}