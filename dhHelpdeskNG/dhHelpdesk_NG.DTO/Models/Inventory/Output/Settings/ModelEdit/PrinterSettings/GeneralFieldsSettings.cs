namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(ModelEditFieldSetting nameFieldSetting, ModelEditFieldSetting manufacturerFieldSetting, ModelEditFieldSetting modelFieldSetting, ModelEditFieldSetting serialNumberFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting NameFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ModelFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting SerialNumberFieldSetting { get; set; }
    }
}