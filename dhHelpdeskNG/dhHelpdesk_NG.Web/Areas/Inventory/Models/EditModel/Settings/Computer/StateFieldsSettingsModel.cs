namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class StateFieldsSettingsModel
    {
        public StateFieldsSettingsModel()
        {
        }

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
            StateFieldSettingModel.IsCopyDisabled = true;
            StolenFieldSettingModel.IsCopyDisabled = true;
            ReplacedWithFieldSettingModel.IsCopyDisabled = true;
            SendBackFieldSettingModel.IsCopyDisabled = true;
            ScrapDateFieldSettingModel.IsCopyDisabled = true;
        }

        [NotNull]
        [LocalizedDisplay("Status")]
        public FieldSettingModel StateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Stulen")]
        public FieldSettingModel StolenFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Ersatt med")]
        public FieldSettingModel ReplacedWithFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Retur")]
        public FieldSettingModel SendBackFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Skrotad datum")]
        public FieldSettingModel ScrapDateFieldSettingModel { get; set; }
    }
}