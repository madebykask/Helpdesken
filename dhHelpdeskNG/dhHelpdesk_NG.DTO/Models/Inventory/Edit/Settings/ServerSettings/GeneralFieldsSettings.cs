namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsSettings
    {
        public WorkstationFieldsSettings(FieldSetting serverNameFieldSetting, FieldSetting manufacturerFieldSetting, FieldSetting descriptionFieldSetting, FieldSetting computerModelFieldSetting, FieldSetting serialNumberFieldSetting)
        {
            this.ServerNameFieldSetting = serverNameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.DescriptionFieldSetting = descriptionFieldSetting;
            this.ComputerModelFieldSetting = computerModelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
        }

        [NotNull]
        public FieldSetting ServerNameFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public FieldSetting DescriptionFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ComputerModelFieldSetting { get; set; }

        [NotNull]
        public FieldSetting SerialNumberFieldSetting { get; set; }
    }
}