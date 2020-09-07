namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Server
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Shared;

    public class ServerFieldsSettingsViewModel
    {
        public ServerFieldsSettingsViewModel()
        {
        }

        public ServerFieldsSettingsViewModel(
            int langaugeId,
            SelectList langauges,
            GeneralFieldsSettingsModel generalFieldsSettingsModel,
            OtherFieldsSettingsModel otherFieldsSettingsModel,
            StateFieldsSettingsModel stateFieldsSettingsModel,
            StorageFieldsSettingsModel storageFieldsSettingsModel,
            ChassisFieldsSettingsModel chassisFieldsSettingsModel,
            InventoryFieldsSettingsModel inventoryFieldsSettingsModel,
            MemoryFieldsSettingsModel memoryFieldsSettingsModel,
            OperatingSystemFieldsSettingsModel operatingSystemFieldsSettingsModel,
            ProccesorFieldsSettingsModel proccesorFieldsSettingsModel,
            PlaceFieldsSettingsModel placeFieldsSettingsModel,
            DocumentFieldsSettingsModel documentFieldsSettingsModel,
            CommunicationFieldsSettingsModel communicationFieldsSettingsModel)
        {
            this.LanguageId = langaugeId;
            this.Languages = langauges;
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
            this.DocumentFieldsSettingsModel = documentFieldsSettingsModel;
            this.CommunicationFieldsSettingsModel = communicationFieldsSettingsModel;
        }

        [IsId]
        public int LanguageId { get; set; }

        [NotNull]
        public SelectList Languages { get; set; }

        [NotNull]
        public GeneralFieldsSettingsModel GeneralFieldsSettingsModel { get;  set; }

        [NotNull]
        public OtherFieldsSettingsModel OtherFieldsSettingsModel { get;  set; }

        [NotNull]
        public StateFieldsSettingsModel StateFieldsSettingsModel { get;  set; }

        [NotNull]
        public StorageFieldsSettingsModel StorageFieldsSettingsModel { get;  set; }

        [NotNull]
        public ChassisFieldsSettingsModel ChassisFieldsSettingsModel { get;  set; }

        [NotNull]
        public InventoryFieldsSettingsModel InventoryFieldsSettingsModel { get;  set; }

        [NotNull]
        public MemoryFieldsSettingsModel MemoryFieldsSettingsModel { get;  set; }

        [NotNull]
        public OperatingSystemFieldsSettingsModel OperatingSystemFieldsSettingsModel { get;  set; }

        [NotNull]
        public ProccesorFieldsSettingsModel ProccesorFieldsSettingsModel { get;  set; }

        [NotNull]
        public PlaceFieldsSettingsModel PlaceFieldsSettingsModel { get;  set; }

        public DocumentFieldsSettingsModel DocumentFieldsSettingsModel { get; set; }

        [NotNull]
        public CommunicationFieldsSettingsModel CommunicationFieldsSettingsModel { get;  set; }
    }
}