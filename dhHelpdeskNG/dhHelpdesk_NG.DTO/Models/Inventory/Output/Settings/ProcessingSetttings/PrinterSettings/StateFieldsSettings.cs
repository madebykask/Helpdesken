namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(
            ProcessingFieldSetting createdDateFieldSetting,
            ProcessingFieldSetting changedDateFieldSetting)
        {
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting CreatedDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ChangedDateFieldSetting { get; set; }
    }
}