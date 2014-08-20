namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerForUpdate : Computer
    {
        public ComputerForUpdate(
            int id,
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
            int? changedByUserId,
            DateTime changedDate)
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
            this.Id = id;
            this.ChangedByUserId = changedByUserId;
            this.ChangedDate = changedDate;
        }

        [IsId]
        public int? ChangedByUserId { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}