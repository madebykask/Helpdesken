namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.InventorySettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldSettings
    {
        public DefaultFieldSettings(
            InventoryFieldSetting departmentFieldSetting,
            InventoryFieldSetting nameFieldSetting,
            InventoryFieldSetting modelFieldSetting,
            InventoryFieldSetting manufacturerFieldSetting,
            InventoryFieldSetting serialNumberFieldSetting,
            InventoryFieldSetting theftMarkFieldSetting,
            InventoryFieldSetting barCodeFieldSetting,
            InventoryFieldSetting purchaseDateFieldSetting,
            InventoryFieldSetting placeFieldSetting,
            InventoryFieldSetting workstationFieldSetting,
            InventoryFieldSetting infoFieldSetting,
            InventoryFieldSetting createdDateFieldSetting,
            InventoryFieldSetting changedDateFieldSetting,
            InventoryFieldSetting syncDateFieldSetting,
            InventoryFieldSetting typeFieldSetting)
        {
            this.DepartmentFieldSetting = departmentFieldSetting;
            this.NameFieldSetting = nameFieldSetting;
            this.ModelFieldSetting = modelFieldSetting;
            this.ManufacturerFieldSetting = manufacturerFieldSetting;
            this.SerialNumberFieldSetting = serialNumberFieldSetting;
            this.TheftMarkFieldSetting = theftMarkFieldSetting;
            this.BarCodeFieldSetting = barCodeFieldSetting;
            this.PurchaseDateFieldSetting = purchaseDateFieldSetting;
            this.PlaceFieldSetting = placeFieldSetting;
            this.WorkstationFieldSetting = workstationFieldSetting;
            this.InfoFieldSetting = infoFieldSetting;
            this.CreatedDateFieldSetting = createdDateFieldSetting;
            this.ChangedDateFieldSetting = changedDateFieldSetting;
            this.SyncDateFieldSetting = syncDateFieldSetting;
            this.TypeFieldSetting = typeFieldSetting;
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
        public InventoryFieldSetting TypeFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting BarCodeFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting PurchaseDateFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting PlaceFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting WorkstationFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting InfoFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting CreatedDateFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting ChangedDateFieldSetting { get; private set; }

        [NotNull]
        public InventoryFieldSetting SyncDateFieldSetting { get; private set; }
    }
}
