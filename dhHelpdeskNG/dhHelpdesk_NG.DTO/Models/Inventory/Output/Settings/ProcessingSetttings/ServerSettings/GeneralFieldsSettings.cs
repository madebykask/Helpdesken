namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(ProcessingFieldSetting serverNameFieldSetting, ProcessingFieldSetting manufacturerFieldSetting, ProcessingFieldSetting descriptionFieldSetting, ProcessingFieldSetting computerModelFieldSetting, ProcessingFieldSetting serialNumberFieldSetting)
        {
            this.ServerNameFieldSetting = serverNameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.DescriptionFieldSetting = descriptionFieldSetting;
            this.ComputerModelFieldSetting = computerModelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting ServerNameFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting DescriptionFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ComputerModelFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting SerialNumberFieldSetting { get; set; }
    }
}