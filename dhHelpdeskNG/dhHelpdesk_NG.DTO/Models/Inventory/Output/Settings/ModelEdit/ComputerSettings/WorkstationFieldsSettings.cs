namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    
    public class WorkstationFieldsSettings
    {
        public WorkstationFieldsSettings(ModelEditFieldSetting computerNameFieldSetting,
            ModelEditFieldSetting manufacturerFieldSetting, ModelEditFieldSetting computerModelFieldSetting,
            ModelEditFieldSetting serialNumberFieldSetting, ModelEditFieldSetting biosVersionFieldSetting,
            ModelEditFieldSetting biosDateFieldSetting, ModelEditFieldSetting theftmarkFieldSetting,
            ModelEditFieldSetting carePackNumberFieldSetting, ModelEditFieldSetting computerTypeFieldSetting,
            ModelEditFieldSetting locationFieldSetting)
        {
            this.ComputerNameFieldSetting = computerNameFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.ComputerModelFieldSetting = computerModelFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
            this.BIOSVersionFieldSetting = biosVersionFieldSetting;
            this.BIOSDateFieldSetting = biosDateFieldSetting;
            this.TheftmarkFieldSetting = theftmarkFieldSetting;
            this.CarePackNumberFieldSetting = carePackNumberFieldSetting;
            this.ComputerTypeFieldSetting = computerTypeFieldSetting;
            this.LocationFieldSetting = locationFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting ComputerNameFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ComputerModelFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting SerialNumberFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting BIOSVersionFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting BIOSDateFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting TheftmarkFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting CarePackNumberFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting ComputerTypeFieldSetting { get; set; }

        [NotNull]
        public ModelEditFieldSetting LocationFieldSetting { get; set; }
    }
}