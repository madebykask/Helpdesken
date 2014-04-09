namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
        public FieldSettingModel StateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel StolenFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ReplacedWithFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel SendBackFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ScrapDateFieldSettingModel { get; set; }
    }
}