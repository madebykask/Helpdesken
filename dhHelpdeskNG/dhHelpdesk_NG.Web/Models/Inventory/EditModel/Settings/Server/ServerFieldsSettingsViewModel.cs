namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Settings.Shared;

    public class ServerFieldsSettingsViewModel
    {
        public ServerFieldsSettingsViewModel()
        {
        }

        public ServerFieldsSettingsViewModel(
            int? customerId,
            int? languageId,
            GeneralFieldsSettingsModel generalFieldsSettingsModel,
            OtherFieldsSettingsModel otherFieldsSettingsModel,
            StateFieldsSettingsModel stateFieldsSettingsModel,
            StorageFieldsSettingsModel storageFieldsSettingsModel,
            ChassisFieldsSettingsModel chassisFieldsSettingsModel,
            InventoryFieldsSettingsModel inventoryFieldsSettingsModel,
            MemoryFieldsSettingsModel memoryFieldsSettingsModel,
            OperatingSystemFieldsSettingsModel operatingSystemFieldsSettingsModel,
            ProccesorFieldsSettingsModel proccesorFieldsSettingsModel,
            PlaceFieldsSettingsModel placeFieldsSettingsModel)
        {
            this.CustomerId = customerId;
            this.LanguageId = languageId;
            this.GeneralFieldsSettingsModel = generalFieldsSettingsModel;
            this.OtherFieldsSettingsModel = otherFieldsSettingsModel;
            this.StateFieldsSettingsModel = stateFieldsSettingsModel;
            this.StorageFieldsSettingsModel = storageFieldsSettingsModel;
            this.ChassisFieldsSettingsModel = chassisFieldsSettingsModel;
            this.InventoryFieldsSettingsModel = inventoryFieldsSettingsModel;
            this.MemoryFieldsSettingsModel = memoryFieldsSettingsModel;
            this.OperatingSystemFieldsSettingsModel = operatingSystemFieldsSettingsModel;
            this.ProccesorFieldsSettingsModel = proccesorFieldsSettingsModel;
            this.PlaceFieldsSettingsModel = placeFieldsSettingsModel;
        }

        [IsId]
        public int? CustomerId { get; private set; }

        [IsId]
        public int? LanguageId { get; private set; }

        [NotNull]
        public GeneralFieldsSettingsModel GeneralFieldsSettingsModel { get; private set; }

        [NotNull]
        public OtherFieldsSettingsModel OtherFieldsSettingsModel { get; private set; }

        [NotNull]
        public StateFieldsSettingsModel StateFieldsSettingsModel { get; private set; }

        [NotNull]
        public StorageFieldsSettingsModel StorageFieldsSettingsModel { get; private set; }

        [NotNull]
        public ChassisFieldsSettingsModel ChassisFieldsSettingsModel { get; private set; }

        [NotNull]
        public InventoryFieldsSettingsModel InventoryFieldsSettingsModel { get; private set; }

        [NotNull]
        public MemoryFieldsSettingsModel MemoryFieldsSettingsModel { get; private set; }

        [NotNull]
        public OperatingSystemFieldsSettingsModel OperatingSystemFieldsSettingsModel { get; private set; }

        [NotNull]
        public ProccesorFieldsSettingsModel ProccesorFieldsSettingsModel { get; private set; }

        [NotNull]
        public PlaceFieldsSettingsModel PlaceFieldsSettingsModel { get; private set; }
    }
}