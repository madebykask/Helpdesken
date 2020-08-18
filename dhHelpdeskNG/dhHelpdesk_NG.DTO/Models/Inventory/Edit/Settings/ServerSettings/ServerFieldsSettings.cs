namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ServerSettings
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings;
    using DH.Helpdesk.Common.ValidationAttributes;

    using PlaceFieldsSettings = DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.SharedSettings.PlaceFieldsSettings;

    public class ServerFieldsSettings : BusinessModel
    {
        private ServerFieldsSettings(
            ModelStates modelState,
            GeneralFieldsSettings generalFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            StateFieldsSettings stateFieldsSettings,
            StorageFieldsSettings storageFieldsSettings,
            ChassisFieldsSettings chassisFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            MemoryFieldsSettings memoryFieldsSettings,
            OperatingSystemFieldsSettings operatingSystemFieldsSettings,
            ProcessorFieldsSettings proccesorFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            DocumentFieldsSettings documentFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings)
        {
            this.State = modelState;
            this.GeneralFieldsSettings = generalFieldsSettings;
            this.OtherFieldsSettings = otherFieldsSettings;
            this.StateFieldsSettings = stateFieldsSettings;
            this.StorageFieldsSettings = storageFieldsSettings;
            this.ChassisFieldsSettings = chassisFieldsSettings;
            this.InventoryFieldsSettings = inventoryFieldsSettings;
            this.MemoryFieldsSettings = memoryFieldsSettings;
            this.OperatingSystemFieldsSettings = operatingSystemFieldsSettings;
            this.ProccesorFieldsSettings = proccesorFieldsSettings;
            this.PlaceFieldsSettings = placeFieldsSettings;
            this.DocumentFieldsSettings = documentFieldsSettings;
            this.CommunicationFieldsSettings = communicationFieldsSettings;
        }

        [IsId]
        [AllowRead(ModelStates.Updated)]
        public int CustomerId { get; private set; }

        [IsId]
        public int LanguageId { get; private set; }

        [AllowRead(ModelStates.Updated)]
        public DateTime ChangedDate { get; private set; }

        [NotNull]
        public GeneralFieldsSettings GeneralFieldsSettings { get; private set; }

        [NotNull]
        public OtherFieldsSettings OtherFieldsSettings { get; private set; }

        [NotNull]
        public StateFieldsSettings StateFieldsSettings { get; private set; }

        [NotNull]
        public StorageFieldsSettings StorageFieldsSettings { get; private set; }

        [NotNull]
        public ChassisFieldsSettings ChassisFieldsSettings { get; private set; }

        [NotNull]
        public InventoryFieldsSettings InventoryFieldsSettings { get; private set; }

        [NotNull]
        public MemoryFieldsSettings MemoryFieldsSettings { get; private set; }

        [NotNull]
        public OperatingSystemFieldsSettings OperatingSystemFieldsSettings { get; private set; }

        [NotNull]
        public ProcessorFieldsSettings ProccesorFieldsSettings { get; private set; }

        [NotNull]
        public PlaceFieldsSettings PlaceFieldsSettings { get; private set; }

        [NotNull]
        public DocumentFieldsSettings DocumentFieldsSettings { get; private set; }

        [NotNull]
        public CommunicationFieldsSettings CommunicationFieldsSettings { get; private set; }

        public static ServerFieldsSettings CreateUpdated(
            int customerId,
            int langaugeId,
            DateTime changedDate,
            GeneralFieldsSettings generalFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            StateFieldsSettings stateFieldsSettings,
            StorageFieldsSettings storageFieldsSettings,
            ChassisFieldsSettings chassisFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            OperatingSystemFieldsSettings operatingSystemFieldsSettings,
            MemoryFieldsSettings memoryFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            DocumentFieldsSettings documentFieldsSettings,
            ProcessorFieldsSettings proccesorFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings)
        {
            var businessModel = new ServerFieldsSettings(
                ModelStates.Updated,
                generalFieldsSettings,
                otherFieldsSettings,
                stateFieldsSettings,
                storageFieldsSettings,
                chassisFieldsSettings,
                inventoryFieldsSettings,
                memoryFieldsSettings,
                operatingSystemFieldsSettings,
                proccesorFieldsSettings,
                placeFieldsSettings,
                documentFieldsSettings,
                communicationFieldsSettings)
                                    {
                                        ChangedDate = changedDate,
                                        CustomerId = customerId,
                                        LanguageId = langaugeId
                                    };

            return businessModel;
        }

        public static ServerFieldsSettings CreateForEdit(
            GeneralFieldsSettings generalFieldsSettings,
            OtherFieldsSettings otherFieldsSettings,
            StateFieldsSettings stateFieldsSettings,
            StorageFieldsSettings storageFieldsSettings,
            ChassisFieldsSettings chassisFieldsSettings,
            InventoryFieldsSettings inventoryFieldsSettings,
            OperatingSystemFieldsSettings operatingSystemFieldsSettings,
            MemoryFieldsSettings memoryFieldsSettings,
            PlaceFieldsSettings placeFieldsSettings,
            DocumentFieldsSettings documentFieldsSettings,
            ProcessorFieldsSettings proccesorFieldsSettings,
            CommunicationFieldsSettings communicationFieldsSettings)
        {
            var businessModel = new ServerFieldsSettings(
                ModelStates.ForEdit,
                generalFieldsSettings,
                otherFieldsSettings,
                stateFieldsSettings,
                storageFieldsSettings,
                chassisFieldsSettings,
                inventoryFieldsSettings,
                memoryFieldsSettings,
                operatingSystemFieldsSettings,
                proccesorFieldsSettings,
                placeFieldsSettings,
                documentFieldsSettings,
                communicationFieldsSettings);

            return businessModel;
        }
    }
}