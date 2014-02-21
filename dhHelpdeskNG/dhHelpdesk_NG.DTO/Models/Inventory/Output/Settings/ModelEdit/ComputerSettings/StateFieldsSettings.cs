namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class StateFieldsSettings
    {
        public StateFieldsSettings(ModelEditFieldSetting stateFieldSetting, ModelEditFieldSetting stolenFieldSetting, ModelEditFieldSetting replacedWithFieldSetting, ModelEditFieldSetting sendBackFieldSetting, ModelEditFieldSetting scrapDateFieldSetting)
        {
            this.StateFieldSetting = stateFieldSetting;
            this.StolenFieldSetting = stolenFieldSetting;
            this.ReplacedWithFieldSetting = replacedWithFieldSetting;
            this.SendBackFieldSetting = sendBackFieldSetting;
            this.ScrapDateFieldSetting = scrapDateFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting StateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting StolenFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ReplacedWithFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting SendBackFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ScrapDateFieldSetting { get; set; }
    }
}