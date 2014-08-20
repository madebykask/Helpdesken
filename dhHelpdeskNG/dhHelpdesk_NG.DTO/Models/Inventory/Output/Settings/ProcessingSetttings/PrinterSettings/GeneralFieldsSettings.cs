namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.PrinterSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(
            ProcessingFieldSetting nameFieldSetting,
            ProcessingFieldSetting manufacturerFieldSetting,
            ProcessingFieldSetting modelFieldSetting,
            ProcessingFieldSetting serialNumberFieldSetting)
        {
            this.NameFieldSetting = nameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting NameFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ModelFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting SerialNumberFieldSetting { get; set; }
    }
}