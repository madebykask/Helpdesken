namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;

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
        public FieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ModelFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel SerialNumberFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel BIOSVersionFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel BIOSDateFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel TheftmarkFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel CarePackNumberFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel ComputerTypeFieldSettingModel { get; set; }

        [NotNull]
        public FieldSettingModel LocationFieldSettingModel { get; set; }
    }
}