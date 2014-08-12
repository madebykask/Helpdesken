namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsSettings
    {
        public WorkstationFieldsSettings(ProcessingFieldSetting computerNameFieldSetting, ProcessingFieldSetting manufacturerFieldSetting, ProcessingFieldSetting computerModelFieldSetting, ProcessingFieldSetting serialNumberFieldSetting, ProcessingFieldSetting biosVersionFieldSetting, ProcessingFieldSetting biosDateFieldSetting, ProcessingFieldSetting theftmarkFieldSetting, ProcessingFieldSetting carePackNumberFieldSetting, ProcessingFieldSetting computerTypeFieldSetting, ProcessingFieldSetting locationFieldSetting)
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
        public ProcessingFieldSetting ComputerNameFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ComputerModelFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting SerialNumberFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting BIOSVersionFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting BIOSDateFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting TheftmarkFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting CarePackNumberFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting ComputerTypeFieldSetting { get; set; }

        [NotNull]
        public ProcessingFieldSetting LocationFieldSetting { get; set; }
    }
}