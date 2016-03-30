namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Printer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class StateFieldsSettingsModel
    {
        public StateFieldsSettingsModel()
        {
        }

        public StateFieldsSettingsModel(FieldSettingModel createdDateFieldSettingModel, FieldSettingModel changedDateFieldSettingModel)
        {
            this.CreatedDateFieldSettingModel = createdDateFieldSettingModel;
            this.ChangedDateFieldSettingModel = changedDateFieldSettingModel;
        }

        [NotNull]
        [LocalizedDisplay("Skapad datum")]
        public FieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Senast ändrad datum")]
        public FieldSettingModel ChangedDateFieldSettingModel { get; set; }
    }
}