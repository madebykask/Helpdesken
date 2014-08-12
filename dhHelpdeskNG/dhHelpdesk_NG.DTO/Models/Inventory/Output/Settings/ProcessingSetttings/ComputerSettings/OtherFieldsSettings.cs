namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(ProcessingFieldSetting infoFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting InfoFieldSetting { get; set; }
    }
}