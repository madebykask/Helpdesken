namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class StateFieldsSettingsModel
    {
        public StateFieldsSettingsModel(
            FieldSettingModel stateFieldSettingModel,
            FieldSettingModel stolenFieldSettingModel,
            FieldSettingModel replacedWithFieldSettingModel,
            FieldSettingModel sendBackFieldSettingModel,
            FieldSettingModel scrapDateFieldSettingModel)
        {
            this.StateFieldSettingModel = stateFieldSettingModel;
            this.StolenFieldSettingModel = stolenFieldSettingModel;
            this.ReplacedWithFieldSettingModel = replacedWithFieldSettingModel;
            this.SendBackFieldSettingModel = sendBackFieldSettingModel;
            this.ScrapDateFieldSettingModel = scrapDateFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("State")]
        public FieldSettingModel StateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Stolen")]
        public FieldSettingModel StolenFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Replaced With")]
        public FieldSettingModel ReplacedWithFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Send Back")]
        public FieldSettingModel SendBackFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Scrap Date")]
        public FieldSettingModel ScrapDateFieldSettingModel { get; set; }
    }
}