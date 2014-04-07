namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Shared;

    public class ServerViewModel
    {
        public ServerViewModel()
        {
        }

        public ServerViewModel(
            int id,
            int? customerId,
            ConfigurableFieldModel<DateTime> createdDate,
            ConfigurableFieldModel<DateTime> changedDate,
            GeneralFieldsModel generalFields,
            OtherFieldsModel otherFields,
            StateFieldsModel stateFields,
            StorageFieldsModel storageFields,
            ChassisFieldsModel chassisFields,
            InventoryFieldsModel inventoryFields,
            MemoryFieldsViewModel memoryFieldsViewModel,
            CommunicationFieldsModel communicationFields,
            OperatingSystemFieldsViewModel operatingSystemFieldsViewModel,
            ProccesorFieldsViewModel proccesorFieldsViewModel,
            PlaceFieldsViewModel placeFieldsViewModel)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.GeneralFieldsModel = generalFields;
            this.OtherFieldsModel = otherFields;
            this.StateFieldsModel = stateFields;
            this.StorageFieldsModel = storageFields;
            this.ChassisFieldsModel = chassisFields;
            this.InventoryFieldsModel = inventoryFields;
            this.MemoryFieldsViewModel = memoryFieldsViewModel;
            this.CommunicationFieldsModel = communicationFields;
            this.OperatingSystemFieldsViewModel = operatingSystemFieldsViewModel;
            this.ProccesorFieldsViewModel = proccesorFieldsViewModel;
            this.PlaceFieldsViewModel = placeFieldsViewModel;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get; private set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get; private set; }

        [NotNull]
        public GeneralFieldsModel GeneralFieldsModel { get; private set; }

        [NotNull]
        public OtherFieldsModel OtherFieldsModel { get; private set; }

        [NotNull]
        public StateFieldsModel StateFieldsModel { get; private set; }

        [NotNull]
        public StorageFieldsModel StorageFieldsModel { get; private set; }

        [NotNull]
        public ChassisFieldsModel ChassisFieldsModel { get; private set; }

        [NotNull]
        public InventoryFieldsModel InventoryFieldsModel { get; private set; }

        [NotNull]
        public MemoryFieldsViewModel MemoryFieldsViewModel { get; private set; }

        [NotNull]
        public CommunicationFieldsModel CommunicationFieldsModel { get; private set; }

        [NotNull]
        public OperatingSystemFieldsViewModel OperatingSystemFieldsViewModel { get; private set; }

        [NotNull]
        public ProccesorFieldsViewModel ProccesorFieldsViewModel { get; private set; }

        [NotNull]
        public PlaceFieldsViewModel PlaceFieldsViewModel { get; private set; }
    }
}