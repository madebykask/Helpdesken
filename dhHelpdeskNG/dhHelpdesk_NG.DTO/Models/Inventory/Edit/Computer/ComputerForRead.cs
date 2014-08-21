namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerForRead : Computer
    {
        public ComputerForRead(
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
            DateTime createdDate,
            DateTime changedDate,
            DateFields dateFields)
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
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
            this.DateFields = dateFields;
        }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        [NotNull]
        public DateFields DateFields { get; private set; }
    }
}