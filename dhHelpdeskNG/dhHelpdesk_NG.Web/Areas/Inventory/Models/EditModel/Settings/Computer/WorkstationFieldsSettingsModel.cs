namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class WorkstationFieldsSettingsModel
    {
        public WorkstationFieldsSettingsModel()
        {
        }

        public WorkstationFieldsSettingsModel(
            FieldSettingModel nameFieldSettingModel,
            FieldSettingModel manufacturerFieldSettingModel,
            FieldSettingModel modelFieldSettingModel,
            FieldSettingModel serialNumberFieldSettingModel,
            FieldSettingModel biosVersionFieldSettingModel,
            FieldSettingModel biosDateFieldSettingModel,
            FieldSettingModel theftmarkFieldSettingModel,
            FieldSettingModel carePackNumberFieldSettingModel,
            FieldSettingModel computerTypeFieldSettingModel,
            FieldSettingModel locationFieldSettingModelModel)
        {
            this.NameFieldSettingModel = nameFieldSettingModel;
            this.ManufacturerFieldSettingModel = manufacturerFieldSettingModel;
            this.ModelFieldSettingModel = modelFieldSettingModel;
            this.SerialNumberFieldSettingModel = serialNumberFieldSettingModel;
            this.BIOSVersionFieldSettingModel = biosVersionFieldSettingModel;
            this.BIOSDateFieldSettingModel = biosDateFieldSettingModel;
            this.TheftmarkFieldSettingModel = theftmarkFieldSettingModel;
            this.CarePackNumberFieldSettingModel = carePackNumberFieldSettingModel;
            this.ComputerTypeFieldSettingModel = computerTypeFieldSettingModel;
            this.LocationFieldSettingModel = locationFieldSettingModelModel;
            this.TheftmarkFieldSettingModel.IsCopyDisabled = true;
        }

        [NotNull]
        [LocalizedDisplay("Datornamn")]
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Tillverkare")]
        public FieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Datormodell")]
        public FieldSettingModel ModelFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Serienummer")]
        public FieldSettingModel SerialNumberFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("BIOS version")]
        public FieldSettingModel BIOSVersionFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("BIOS datum")]
        public FieldSettingModel BIOSDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Stöldmärkning")]
        public FieldSettingModel TheftmarkFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("CarePack Nummer")]
        public FieldSettingModel CarePackNumberFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Datortyp")]
        public FieldSettingModel ComputerTypeFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Placering")]
        public FieldSettingModel LocationFieldSettingModel { get; set; }
    }
}