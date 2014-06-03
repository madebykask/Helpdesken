namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class WorkstationFieldsSettingsModel
    {
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
        }

        [NotNull]
        [LocalizedDisplay("Name")]
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Manufacturer")]
        public FieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Model")]
        public FieldSettingModel ModelFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Serial Number")]
        public FieldSettingModel SerialNumberFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("BIOS Version")]
        public FieldSettingModel BIOSVersionFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("BIOS Date")]
        public FieldSettingModel BIOSDateFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Theftmark")]
        public FieldSettingModel TheftmarkFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Care Pack Number")]
        public FieldSettingModel CarePackNumberFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Computer Type")]
        public FieldSettingModel ComputerTypeFieldSettingModel { get; set; }

        [NotNull]
        [LocalizedDisplay("Location")]
        public FieldSettingModel LocationFieldSettingModel { get; set; }
    }
}