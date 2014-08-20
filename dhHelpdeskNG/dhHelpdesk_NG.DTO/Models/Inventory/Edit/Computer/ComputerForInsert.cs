namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerForInsert : Computer
    {
        public ComputerForInsert(
            CommunicationFields communicationFields,
            ContactFields contactFields,
            ContactInformationFields contactInformationFields,
            ContractFields contractFields,
            GraphicsFields graphicsFields,
            OtherFields otherFields,
            PlaceFields placeFields,
            SoundFields soundFields,
            StateFields stateFields,
            ChassisFields chassisFields,
            InventoryFields inventoryFields,
            MemoryFields memoryFields,
            OperatingSystemFields operatingSystemFields,
            OrganizationFields organizationFields,
            ProcessorFields proccesorFields,
            WorkstationFields workstationFields,
            int? customerId,
            int? changedByUserId,
            DateTime createdDate)
            : base(
                communicationFields,
                contactFields,
                contactInformationFields,
                contractFields,
                graphicsFields,
                otherFields,
                placeFields,
                soundFields,
                stateFields,
                chassisFields,
                inventoryFields,
                memoryFields,
                operatingSystemFields,
                organizationFields,
                proccesorFields,
                workstationFields)
        {
            this.CustomerId = customerId;
            this.ChangedByUserId = changedByUserId;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int? CustomerId { get; private set; }

        [IsId]
        public int? ChangedByUserId { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}