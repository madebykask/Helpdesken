namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DefaultFieldSettingsModel
    {
        public DefaultFieldSettingsModel()
        {
        }

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
            InventoryFieldSettingModel infoFieldSettingModel,
            InventoryFieldSettingModel createdDateFieldSettingModel,
            InventoryFieldSettingModel changedDateFieldSettingModel)
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
            this.CreatedDateFieldSettingModel = createdDateFieldSettingModel;
            this.ChangedDateFieldSettingModel = changedDateFieldSettingModel;
        }

        [NotNull]
        public InventoryFieldSettingModel DepartmentFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel NameFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel ModelFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel ManufacturerFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel SerialNumberFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel TheftMarkFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel BarCodeFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel PurchaseDateFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel PlaceFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel WorkstationFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel InfoFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel CreatedDateFieldSettingModel { get; set; }

        [NotNull]
        public InventoryFieldSettingModel ChangedDateFieldSettingModel { get; set; }
    }
}
