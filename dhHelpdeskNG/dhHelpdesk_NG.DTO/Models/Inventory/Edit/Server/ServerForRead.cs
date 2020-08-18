namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ServerForRead : Server
    {
        public ServerForRead(
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
            StateFields stateFields,
            DateTime changedDate,
            DateTime createdDate)
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
            this.StateFields = stateFields;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
        }

        [NotNull]
        public StateFields StateFields { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}