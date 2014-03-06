namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSetting stateFieldSetting, FieldSetting stolenFieldSetting, FieldSetting replacedWithFieldSetting, FieldSetting sendBackFieldSetting, FieldSetting scrapDateFieldSetting)
        {
            this.StateFieldSetting = stateFieldSetting;
            this.StolenFieldSetting = stolenFieldSetting;
            this.ReplacedWithFieldSetting = replacedWithFieldSetting;
            this.SendBackFieldSetting = sendBackFieldSetting;
            this.ScrapDateFieldSetting = scrapDateFieldSetting;
        }

        [NotNull]
        public FieldSetting StateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting StolenFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ReplacedWithFieldSetting { get; set; }

        [NotNull]
        public FieldSetting SendBackFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ScrapDateFieldSetting { get; set; }
    }
}