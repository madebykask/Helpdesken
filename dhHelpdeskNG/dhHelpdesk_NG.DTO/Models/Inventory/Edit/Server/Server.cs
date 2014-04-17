namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class Server : BusinessModel
    {
        private Server(
            ModelStates modelStates,
            GeneralFields generalFields,
            OtherFields otherFields,
            StateFields stateFields,
            StorageFields storageFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            OperatingSystemFields operatingSystemFields,
            MemoryFields memoryFields,
            PlaceFields placeFields,
            ProcessorFields proccesorFields,
            CommunicationFields communicationFields)
            : base(modelStates)
        {
            this.GeneralFields = generalFields;
            this.OtherFields = otherFields;
            this.StateFields = stateFields;
            this.StorageFields = storageFields;
            this.ChassisFields = chassisFields;
            this.InventoryFields = inventoryFields;
            this.OperatingSystemFields = operatingSystemFields;
            this.MemoryFields = memoryFields;
            this.PlaceFields = placeFields;
            this.ProccesorFields = proccesorFields;
            this.CommunicationFields = communicationFields;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [AllowRead(ModelStates.Updated | ModelStates.ForEdit)]
        public DateTime CreatedDate { get; private set; }

        [AllowRead(ModelStates.Updated | ModelStates.ForEdit)]
        public DateTime ChangedDate { get; private set; }

        [NotNull]
        public GeneralFields GeneralFields { get; private set; }

        [NotNull]
        public OtherFields OtherFields { get; private set; }

        [NotNull]
        public StateFields StateFields { get; private set; }

        [NotNull]
        public StorageFields StorageFields { get; private set; }

        [NotNull]
        public ChassisFields ChassisFields { get; private set; }

        [NotNull]
        public InventoryFields InventoryFields { get; private set; }

        [NotNull]
        public MemoryFields MemoryFields { get; private set; }

        [NotNull]
        public OperatingSystemFields OperatingSystemFields { get; private set; }

        [NotNull]
        public ProcessorFields ProccesorFields { get; private set; }

        [NotNull]
        public PlaceFields PlaceFields { get; private set; }

        [NotNull]
        public CommunicationFields CommunicationFields { get; private set; }

        public static Server CreateNew(
            int customerId,
            GeneralFields generalFields,
            OtherFields otherFields,
            StateFields stateFields,
            StorageFields storageFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            OperatingSystemFields operatingSystemFields,
            MemoryFields memoryFields,
            PlaceFields placeFields,
            ProcessorFields proccesorFields,
            DateTime createdDate,
            CommunicationFields communicationFields)
        {
            var businessModel = new Server(
                ModelStates.Created,
                generalFields,
                otherFields,
                stateFields,
                storageFields,
                chassisFields,
                inventoryFields,
                operatingSystemFields,
                memoryFields,
                placeFields,
                proccesorFields,
                communicationFields) { CustomerId = customerId, CreatedDate = createdDate };

            return businessModel;
        }

        public static Server CreateUpdated(
            int id,
            GeneralFields generalFields,
            OtherFields otherFields,
            StateFields stateFields,
            StorageFields storageFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            OperatingSystemFields operatingSystemFields,
            MemoryFields memoryFields,
            PlaceFields placeFields,
            ProcessorFields proccesorFields,
            DateTime changedDate,
            CommunicationFields communicationFields)
        {
            var businessModel = new Server(
                ModelStates.Updated,
                generalFields,
                otherFields,
                stateFields,
                storageFields,
                chassisFields,
                inventoryFields,
                operatingSystemFields,
                memoryFields,
                placeFields,
                proccesorFields,
                communicationFields) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static Server CreateForEdit(
            int id,
            GeneralFields generalFields,
            OtherFields otherFields,
            StateFields stateFields,
            StorageFields storageFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            OperatingSystemFields operatingSystemFields,
            MemoryFields memoryFields,
            PlaceFields placeFields,
            ProcessorFields proccesorFields,
            DateTime createdDate,
            DateTime changedDate,
            CommunicationFields communicationFields)
        {
            var businessModel = new Server(
                ModelStates.ForEdit,
                generalFields,
                otherFields,
                stateFields,
                storageFields,
                chassisFields,
                inventoryFields,
                operatingSystemFields,
                memoryFields,
                placeFields,
                proccesorFields,
                communicationFields) { Id = id, CreatedDate = createdDate, ChangedDate = changedDate };

            return businessModel;
        }
    }
}