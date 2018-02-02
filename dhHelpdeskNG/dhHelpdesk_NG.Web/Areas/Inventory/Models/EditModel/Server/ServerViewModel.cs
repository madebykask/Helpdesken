namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Server
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Shared;

    public class ServerViewModel
    {
        public ServerViewModel()
        {
        }

        public ServerViewModel(
            bool isOperationObject,
            GeneralFieldsModel generalFields,
            OtherFieldsModel otherFields,
            StateFieldsModel stateFields,
            StorageFieldsModel storageFields,
            ChassisFieldsModel chassisFields,
            InventoryFieldsModel inventoryFields,
            MemoryFieldsViewModel memoryFieldsViewModel,
            CommunicationFieldsViewModel communicationFieldsViewModel,
            OperatingSystemFieldsViewModel operatingSystemFieldsViewModel,
            ProccesorFieldsViewModel proccesorFieldsViewModel,
            PlaceFieldsViewModel placeFieldsViewModel)
        {
            this.IsOperationObject = isOperationObject;
            this.GeneralFieldsModel = generalFields;
            this.OtherFieldsModel = otherFields;
            this.StateFieldsModel = stateFields;
            this.StorageFieldsModel = storageFields;
            this.ChassisFieldsModel = chassisFields;
            this.InventoryFieldsModel = inventoryFields;
            this.MemoryFieldsViewModel = memoryFieldsViewModel;
            this.CommunicationFieldsViewModel = communicationFieldsViewModel;
            this.OperatingSystemFieldsViewModel = operatingSystemFieldsViewModel;
            this.ProccesorFieldsViewModel = proccesorFieldsViewModel;
            this.PlaceFieldsViewModel = placeFieldsViewModel;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int CustomerId { get; set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get;  set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get;  set; }

        public bool IsOperationObject { get; set; }

        [NotNull]
        public GeneralFieldsModel GeneralFieldsModel { get;  set; }

        [NotNull]
        public OtherFieldsModel OtherFieldsModel { get;  set; }

        [NotNull]
        public StateFieldsModel StateFieldsModel { get;  set; }

        [NotNull]
        public StorageFieldsModel StorageFieldsModel { get;  set; }

        [NotNull]
        public ChassisFieldsModel ChassisFieldsModel { get;  set; }

        [NotNull]
        public InventoryFieldsModel InventoryFieldsModel { get;  set; }

        [NotNull]
        public MemoryFieldsViewModel MemoryFieldsViewModel { get;  set; }

        [NotNull]
        public CommunicationFieldsViewModel CommunicationFieldsViewModel { get;  set; }

        [NotNull]
        public OperatingSystemFieldsViewModel OperatingSystemFieldsViewModel { get;  set; }

        [NotNull]
        public ProccesorFieldsViewModel ProccesorFieldsViewModel { get;  set; }

        [NotNull]
        public PlaceFieldsViewModel PlaceFieldsViewModel { get;  set; }

        public bool IsForDialog { get; set; }
    }
}