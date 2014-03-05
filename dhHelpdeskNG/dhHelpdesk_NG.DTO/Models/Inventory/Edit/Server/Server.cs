namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class Server
    {
        private Server(GeneralFields generalFields, OtherFields otherFields, StateFields stateFields, StorageFields storageFields, ChassisFields chassisFields, InventoryFields inventoryFields, OperatingSystemFields operatingSystemFields, MemoryFields memoryFields, OrganizationFields organizationFields, PlaceFields placeFields, ProccesorFields proccesorFields)
        {
            this.GeneralFields = generalFields;
            this.OtherFields = otherFields;
            this.StateFields = stateFields;
            this.StorageFields = storageFields;
            this.ChassisFields = chassisFields;
            this.InventoryFields = inventoryFields;
            this.OperatingSystemFields = operatingSystemFields;
            this.MemoryFields = memoryFields;
            this.OrganizationFields = organizationFields;
            this.PlaceFields = placeFields;
            this.ProccesorFields = proccesorFields;
        }

        [IsId]
        public int Id { get; private set; }

        [IsId]
        public int? CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

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
        public OrganizationFields OrganizationFields { get; private set; }

        [NotNull]
        public ProccesorFields ProccesorFields { get; private set; }

        [NotNull]
        public PlaceFields PlaceFields { get; private set; }

        public static Server CreateNew(int? customerId, GeneralFields generalFields, OtherFields otherFields, StateFields stateFields, StorageFields storageFields, ChassisFields chassisFields, InventoryFields inventoryFields, OperatingSystemFields operatingSystemFields, MemoryFields memoryFields, OrganizationFields organizationFields, PlaceFields placeFields, ProccesorFields proccesorFields, DateTime createdDate)
        {
            var businessModel = new Server(generalFields, otherFields, stateFields, storageFields, chassisFields, inventoryFields, operatingSystemFields, memoryFields, organizationFields, placeFields, proccesorFields) { CustomerId = customerId, CreatedDate = createdDate };

            return businessModel;
        }

        public static Server CreateUpdated(int id, GeneralFields generalFields, OtherFields otherFields, StateFields stateFields, StorageFields storageFields, ChassisFields chassisFields, InventoryFields inventoryFields, OperatingSystemFields operatingSystemFields, MemoryFields memoryFields, OrganizationFields organizationFields, PlaceFields placeFields, ProccesorFields proccesorFields, DateTime changedDate)
        {
            var businessModel = new Server(generalFields, otherFields, stateFields, storageFields, chassisFields, inventoryFields, operatingSystemFields, memoryFields, organizationFields, placeFields, proccesorFields) { Id = id, ChangedDate = changedDate };

            return businessModel;
        }

        public static Server CreateForEdit(int id, GeneralFields generalFields, OtherFields otherFields, StateFields stateFields, StorageFields storageFields, ChassisFields chassisFields, InventoryFields inventoryFields, OperatingSystemFields operatingSystemFields, MemoryFields memoryFields, OrganizationFields organizationFields, PlaceFields placeFields, ProccesorFields proccesorFields, DateTime createdDate, DateTime changedDate)
        {
            var businessModel = new Server(generalFields, otherFields, stateFields, storageFields, chassisFields, inventoryFields, operatingSystemFields, memoryFields, organizationFields, placeFields, proccesorFields) { Id = id, CreatedDate = createdDate, ChangedDate = changedDate };

            return businessModel;
        }
    }
}