namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsSettings
    {
        public WorkstationFieldsSettings(FieldSetting computerNameFieldSetting, FieldSetting manufacturerFieldSetting, FieldSetting computerModelFieldSetting, FieldSetting serialNumberFieldSetting, FieldSetting biosVersionFieldSetting, FieldSetting biosDateFieldSetting, FieldSetting theftmarkFieldSetting, FieldSetting carePackNumberFieldSetting, FieldSetting computerTypeFieldSetting, FieldSetting locationFieldSetting)
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
        public FieldSetting ComputerNameFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ManufacturerFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ComputerModelFieldSetting { get; set; }

        [NotNull]
        public FieldSetting SerialNumberFieldSetting { get; set; }

        [NotNull]
        public FieldSetting BIOSVersionFieldSetting { get; set; }

        [NotNull]
        public FieldSetting BIOSDateFieldSetting { get; set; }

        [NotNull]
        public FieldSetting TheftmarkFieldSetting { get; set; }

        [NotNull]
        public FieldSetting CarePackNumberFieldSetting { get; set; }

        [NotNull]
        public FieldSetting ComputerTypeFieldSetting { get; set; }

        [NotNull]
        public FieldSetting LocationFieldSetting { get; set; }
    }
}