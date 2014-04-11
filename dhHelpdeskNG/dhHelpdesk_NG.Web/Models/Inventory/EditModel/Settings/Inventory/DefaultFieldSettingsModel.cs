namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings;

    public class DefaultFieldSettingsModel
    {
        public DefaultFieldSettingsModel(
            InventoryFieldSettingModel departmentFieldSettingModel,
            InventoryFieldSettingModel nameFieldSettingModel,
            InventoryFieldSettingModel modelFieldSettingModel,
            InventoryFieldSettingModel manufacturerFieldSettingModel,
            InventoryFieldSettingModel serialNumberFieldSettingModel,
            InventoryFieldSettingModel theftMarkFieldSettingModel,
            InventoryFieldSettingModel barCodeFieldSettingModel,
            InventoryFieldSettingModel purchaseDateFieldSettingModel,
            InventoryFieldSettingModel placeFieldSettingModel,
            InventoryFieldSettingModel workstationFieldSettingModel,
            InventoryFieldSettingModel infoFieldSettingModel)
        {
            this.DepartmentFieldSettingModel = departmentFieldSettingModel;
            this.NameFieldSettingModel = nameFieldSettingModel;
            this.ModelFieldSettingModel = modelFieldSettingModel;
            this.ManufacturerFieldSettingModel = manufacturerFieldSettingModel;
            this.SerialNumberFieldSettingModel = serialNumberFieldSettingModel;
            this.TheftMarkFieldSettingModel = theftMarkFieldSettingModel;
            this.BarCodeFieldSettingModel = barCodeFieldSettingModel;
            this.PurchaseDateFieldSettingModel = purchaseDateFieldSettingModel;
            this.PlaceFieldSettingModel = placeFieldSettingModel;
            this.WorkstationFieldSettingModel = workstationFieldSettingModel;
            this.InfoFieldSettingModel = infoFieldSettingModel;
        }

        [NotNull]
        public InventoryFieldSettingModel DepartmentFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel NameFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel ModelFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel ManufacturerFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel SerialNumberFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel TheftMarkFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel BarCodeFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel PurchaseDateFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel PlaceFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel WorkstationFieldSettingModel { get; private set; }

        [NotNull]
        public InventoryFieldSettingModel InfoFieldSettingModel { get; private set; }
    }
}
