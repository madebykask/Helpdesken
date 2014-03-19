namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldSettings
    {
        public DefaultFieldSettings(InventoryFieldSetting departmentFieldSetting, InventoryFieldSetting nameFieldSetting, InventoryFieldSetting modelFieldSetting, InventoryFieldSetting manufacturerFieldSetting, InventoryFieldSetting serialNumberFieldSetting, InventoryFieldSetting theftMarkFieldSetting, InventoryFieldSetting purchaseDateFieldSetting, InventoryFieldSetting placeFieldSetting, InventoryFieldSetting workstationFieldSetting, InventoryFieldSetting infoFieldSetting)
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
        public InventoryFieldSetting DepartmentFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting NameFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting ModelFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting ManufacturerFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting SerialNumberFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting TheftMarkFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting PurchaseDateFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting PlaceFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting WorkstationFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting InfoFieldSetting { get; private set; }
    }
}
