namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldSettings
    {
        public DefaultFieldSettings(
            ModelEditFieldSetting departmentFieldSetting,
            ModelEditFieldSetting nameFieldSetting,
            ModelEditFieldSetting modelFieldSetting,
            ModelEditFieldSetting manufacturerFieldSetting,
            ModelEditFieldSetting serialNumberFieldSetting,
            ModelEditFieldSetting theftMarkFieldSetting,
            ModelEditFieldSetting purchaseDateFieldSetting,
            ModelEditFieldSetting placeFieldSetting,
            ModelEditFieldSetting workstationFieldSetting,
            ModelEditFieldSetting infoFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.NameFieldSetting = nameFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
            this.TheftMarkFieldSetting = theftMarkFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
            this.PlaceFieldSetting = placeFieldSetting;
            this.WorkstationFieldSetting = workstationFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting DepartmentFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting NameFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting ModelFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting ManufacturerFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting SerialNumberFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting TheftMarkFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting PurchaseDateFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting PlaceFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting WorkstationFieldSetting { get; private set; }

        [NotNull]
        public ModelEditFieldSetting InfoFieldSetting { get; private set; }
    }
}
