namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(FieldSettingOverview stateFieldSetting, FieldSettingOverview stolenFieldSetting, FieldSettingOverview replacedWithFieldSetting, FieldSettingOverview sendBackFieldSetting, FieldSettingOverview scrapDateFieldSetting)
        {
            this.StateFieldSetting = stateFieldSetting;
            this.StolenFieldSetting = stolenFieldSetting;
            this.ReplacedWithFieldSetting = replacedWithFieldSetting;
            this.SendBackFieldSetting = sendBackFieldSetting;
            this.ScrapDateFieldSetting = scrapDateFieldSetting;
        }

        [NotNull]
        public FieldSettingOverview StateFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview StolenFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ReplacedWithFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview SendBackFieldSetting { get; set; }

        [NotNull]
        public FieldSettingOverview ScrapDateFieldSetting { get; set; }
    }
}