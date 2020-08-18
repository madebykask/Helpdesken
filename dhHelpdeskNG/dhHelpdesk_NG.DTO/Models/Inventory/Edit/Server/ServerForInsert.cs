namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServerForInsert : Server
    {
        public ServerForInsert(
            bool isOperationObject,
            GeneralFields generalFields,
            OtherFields otherFields,
            StorageFields storageFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            OperatingSystemFields operatingSystemFields,
            MemoryFields memoryFields,
            PlaceFields placeFields,
            DocumentFields documentFields,
            ProcessorFields proccesorFields,
            CommunicationFields communicationFields,
            int customerId,
            DateTime createdDate,
            int? changedByUserId)
            : base(
                isOperationObject,
                generalFields,
                otherFields,
                storageFields,
                chassisFields,
                inventoryFields,
                operatingSystemFields,
                memoryFields,
                placeFields,
                documentFields,
                proccesorFields,
                communicationFields)
        {
            this.CustomerId = customerId;
            this.CreatedDate = createdDate;
            this.ChangedByUserId = changedByUserId;
        }

        [IsId]
        public int CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        [IsId]
        public int? ChangedByUserId { get; private set; }
    }
}