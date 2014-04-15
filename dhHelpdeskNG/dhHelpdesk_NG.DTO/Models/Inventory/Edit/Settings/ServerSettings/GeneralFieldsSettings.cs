namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(
            FieldSetting nameFieldSetting,
            FieldSetting manufacturerFieldSetting,
            FieldSetting descriptionFieldSetting,
            FieldSetting modelFieldSetting,
            FieldSetting serialNumberFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.DescriptionFieldSetting = descriptionFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public FieldSetting NameFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public FieldSetting DescriptionFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ModelFieldSetting { get; set; }

        [NotNull]
        public FieldSetting SerialNumberFieldSetting { get; set; }
    }
}