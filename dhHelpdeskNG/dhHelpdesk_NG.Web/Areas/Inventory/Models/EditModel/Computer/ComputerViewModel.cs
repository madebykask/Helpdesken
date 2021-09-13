namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Computer
{
	using System;

	using DH.Helpdesk.Common.Types;
	using DH.Helpdesk.Common.ValidationAttributes;
	using System.Collections.Generic;

	public class ComputerViewModel
    {
        public ComputerViewModel()
        {
        }

        public ComputerViewModel(
            DateFieldsModel dateFields,
            CommunicationFieldsViewModel communicationFieldsViewModel,
            ContactFieldsModel contactFields,
            ContactInformationFieldsModel contactInformationFields,
            ContractFieldsViewModel contractFieldsViewModel,
            GraphicsFieldsModel graphicsFields,
            OtherFieldsModel otherFields,
            PlaceFieldsViewModel placeFieldsViewModel,
            SoundFieldsModel soundFields,
            StateFieldsViewModel stateFieldsViewModel,
            ChassisFieldsModel chassisFields,
            InventoryFieldsModel inventoryFields,
            MemoryFieldsViewModel memoryFieldsViewModel,
            OperatingSystemFieldsViewModel operatingSystemFieldsViewModel,
            OrganizationFieldsViewModel organizationFieldsViewModel,
            ProccesorFieldsViewModel proccesorFieldsViewModel,
            WorkstationFieldsViewModel workstationFieldsViewModel,
			List<string> fileUploadWhiteList)
        {
            this.DateFieldsModel = dateFields;
            this.CommunicationFieldsViewModel = communicationFieldsViewModel;
            this.ContactFieldsModel = contactFields;
            this.ContactInformationFieldsModel = contactInformationFields;
            this.ContractFieldsViewModel = contractFieldsViewModel;
            this.GraphicsFieldsModel = graphicsFields;
            this.OtherFieldsModel = otherFields;
            this.PlaceFieldsViewModel = placeFieldsViewModel;
            this.SoundFieldsModel = soundFields;
            this.StateFieldsViewModel = stateFieldsViewModel;
            this.ChassisFieldsModel = chassisFields;
            this.InventoryFieldsModel = inventoryFields;
            this.MemoryFieldsViewModel = memoryFieldsViewModel;
            this.OperatingSystemFieldsViewModel = operatingSystemFieldsViewModel;
            this.OrganizationFieldsViewModel = organizationFieldsViewModel;
            this.ProccesorFieldsViewModel = proccesorFieldsViewModel;
            this.WorkstationFieldsViewModel = workstationFieldsViewModel;
			this.FileUploadWhiteList = fileUploadWhiteList;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? CustomerId { get; set; }

        public string DocumentFileKey { get; set; }

        public ConfigurableFieldModel<DateTime> CreatedDate { get;  set; }

        public ConfigurableFieldModel<DateTime> ChangedDate { get;  set; }

        public UserName ChangedByUserName { get; set; }

        [NotNull]
        public DateFieldsModel DateFieldsModel { get;  set; }

        [NotNull]
        public CommunicationFieldsViewModel CommunicationFieldsViewModel { get;  set; }

        [NotNull]
        public ContactFieldsModel ContactFieldsModel { get;  set; }

        [NotNull]
        public ContactInformationFieldsModel ContactInformationFieldsModel { get;  set; }

        [NotNull]
        public ContractFieldsViewModel ContractFieldsViewModel { get;  set; }

        [NotNull]
        public GraphicsFieldsModel GraphicsFieldsModel { get;  set; }

        [NotNull]
        public OtherFieldsModel OtherFieldsModel { get;  set; }

        [NotNull]
        public PlaceFieldsViewModel PlaceFieldsViewModel { get;  set; }

        [NotNull]
        public SoundFieldsModel SoundFieldsModel { get;  set; }

        [NotNull]
        public StateFieldsViewModel StateFieldsViewModel { get;  set; }

        [NotNull]
        public ChassisFieldsModel ChassisFieldsModel { get;  set; }

        [NotNull]
        public InventoryFieldsModel InventoryFieldsModel { get;  set; }

        [NotNull]
        public MemoryFieldsViewModel MemoryFieldsViewModel { get;  set; }

        [NotNull]
        public OperatingSystemFieldsViewModel OperatingSystemFieldsViewModel { get;  set; }

        [NotNull]
        public OrganizationFieldsViewModel OrganizationFieldsViewModel { get;  set; }

        [NotNull]
        public ProccesorFieldsViewModel ProccesorFieldsViewModel { get;  set; }

        [NotNull]
        public WorkstationFieldsViewModel WorkstationFieldsViewModel { get;  set; }

		public List<string> FileUploadWhiteList { get; set; }

		public bool IsForDialog { get; set; }
        public string UserId { get; set; }
        public bool IsCopy { get; set; }
    }
}