namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServerForUpdate : Server
    {
        public ServerForUpdate(
            int id,
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
            DateTime changedDate,
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
            this.Id = id;
            this.ChangedDate = changedDate;
            this.ChangedByUserId = changedByUserId;
        }

        public DateTime ChangedDate { get; private set; }

        [IsId]
        public int? ChangedByUserId { get; private set; }
    }
}