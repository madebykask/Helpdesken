namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Server
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServerOverview
    {
        public ServerOverview(
            int id,
            int? customerId,
            DateTime createdDate,
            DateTime changedDate,
            Edit.Server.GeneralFields generalFields,
            Edit.Server.OtherFields otherFields,
            Edit.Server.StateFields stateFields,
            Edit.Server.StorageFields storageFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            MemoryFields memoryFields,
            OperatingSystemFields operatingSystemFields,
            ProcessorFields proccesorFields,
            PlaceFields placeFields)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.GeneralFields = generalFields;
            this.OtherFields = otherFields;
            this.StateFields = stateFields;
            this.StorageFields = storageFields;
            this.ChassisFields = chassisFields;
            this.InventoryFields = inventoryFields;
            this.MemoryFields = memoryFields;
            this.OperatingSystemFields = operatingSystemFields;
            this.ProccesorFields = proccesorFields;
            this.PlaceFields = placeFields;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        [NotNull]
        public Edit.Server.GeneralFields GeneralFields { get; private set; }

        [NotNull]
        public Edit.Server.OtherFields OtherFields { get; private set; }

        [NotNull]
        public Edit.Server.StateFields StateFields { get; private set; }

        [NotNull]
        public Edit.Server.StorageFields StorageFields { get; private set; }

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
    }
}
