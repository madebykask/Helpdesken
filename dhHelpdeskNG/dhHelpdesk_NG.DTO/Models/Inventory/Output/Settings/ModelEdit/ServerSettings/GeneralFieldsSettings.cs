namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class GeneralFieldsSettings
    {
        public GeneralFieldsSettings(ModelEditFieldSetting serverNameFieldSetting, ModelEditFieldSetting manufacturerFieldSetting, ModelEditFieldSetting descriptionFieldSetting, ModelEditFieldSetting computerModelFieldSetting, ModelEditFieldSetting serialNumberFieldSetting)
        {
            this.ServerNameFieldSetting = serverNameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.DescriptionFieldSetting = descriptionFieldSetting;
            this.ComputerModelFieldSetting = computerModelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting ServerNameFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting DescriptionFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ComputerModelFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting SerialNumberFieldSetting { get; set; }
    }
}