namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(ProcessingFieldSetting stateFieldSetting, ProcessingFieldSetting stolenFieldSetting, ProcessingFieldSetting replacedWithFieldSetting, ProcessingFieldSetting sendBackFieldSetting, ProcessingFieldSetting scrapDateFieldSetting)
        {
            this.StateFieldSetting = stateFieldSetting;
            this.StolenFieldSetting = stolenFieldSetting;
            this.ReplacedWithFieldSetting = replacedWithFieldSetting;
            this.SendBackFieldSetting = sendBackFieldSetting;
            this.ScrapDateFieldSetting = scrapDateFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting StateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting StolenFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ReplacedWithFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting SendBackFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ScrapDateFieldSetting { get; set; }
    }
}