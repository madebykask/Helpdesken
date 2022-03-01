namespace DH.Helpdesk.Dal.Attributes.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DH.Helpdesk.Dal.Enums.Inventory.Computer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    using PostSharp.Aspects;

    using PlaceFields = DH.Helpdesk.Dal.Enums.Inventory.Computer.PlaceFields;

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingComputerSettingsAttribute : OnMethodBoundaryAspect
    {
        #region Fields

        private readonly string customerIdParameterName;

        #endregion

        #region Constructors and Destructors

        public CreateMissingComputerSettingsAttribute(string customerIdParameter)
        {
            this.customerIdParameterName = customerIdParameter;
        }

        #endregion

        #region Public Methods and Operators

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodParameters = args.Method.GetParameters();
            var customerIdParameter = methodParameters.Single(p => p.Name == this.customerIdParameterName);
            var customerIdParameterIndex = customerIdParameter.Position;
            var customerId = (int)args.Arguments[customerIdParameterIndex];

            var dbcontext = new DatabaseFactory().Get();
            var settings = dbcontext.ComputerFieldSettings;
            var settingNames = settings.Where(s => s.Customer_Id == customerId).Select(s => s.ComputerField).ToList();

            CreateMissingWorkstationFieldsSettings(customerId, settingNames, settings);
            CreateMissingChassisFieldsSettings(customerId, settingNames, settings);
            CreateMissingInventoringFieldsSettings(customerId, settingNames, settings);
            CreateMissingOperatingSystemFieldsSettings(customerId, settingNames, settings);
            CreateMissingProcessorFieldsSettings(customerId, settingNames, settings);
            CreateMissingMemoryFieldsSettings(customerId, settingNames, settings);
            CreateMissingCommunicationFieldsSettings(customerId, settingNames, settings);
            CreateMissingGraphicsFieldsSettings(customerId, settingNames, settings);
            CreateMissingSoundFieldsSettings(customerId, settingNames, settings);
            CreateMissingContractFieldsSettings(customerId, settingNames, settings);
            CreateMissingOtherFieldsSettings(customerId, settingNames, settings);
            CreateMissingContactInformationFieldsSettings(customerId, settingNames, settings);
            CreateMissingOrganizationFieldsSettings(customerId, settingNames, settings);
            CreateMissingPlaceFieldsSettings(customerId, settingNames, settings);
            CreateMissingContactFieldsSettings(customerId, settingNames, settings);
            CreateMissingStateFieldsSettings(customerId, settingNames, settings);
            CreateMissingDateFieldsSettings(customerId, settingNames, settings);

            dbcontext.Commit();
            base.OnEntry(args);
        }

        #endregion

        #region Methods

        private static ComputerFieldSettings CreateDefaultSetting(string fieldName, int customerId, bool isCopy = true)
        {
            return new ComputerFieldSettings
                   {
                       ComputerField = fieldName,
                       CreatedDate = DateTime.Now,
                       ChangedDate = DateTime.Now, // todo
                       Customer_Id = customerId,
                       Label = fieldName,
                       Label_ENG = fieldName,
                       Required = 0,
                       ReadOnly = 0,
                       Copy = isCopy ? 1 : 0,
                       Show = 0,
                       ShowInList = 0,
                       FieldHelp = string.Empty
                   };
        }

        private static void CreateMissingWorkstationFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(WorkstationFields.Name, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.Manufacturer, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.Model, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.SerialNumber, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.BIOSDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.BIOSVersion, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.Theftmark, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.CarePackNumber, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.ComputerType, customerId, settingNames, settings);
            CreateSettingIfNeeded(WorkstationFields.Location, customerId, settingNames, settings);
        }

        private static void CreateMissingChassisFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(
                ChassisFields.Chassis, customerId, settingNames, settings);
        }

        private static void CreateMissingInventoringFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(InventoryFields.BarCode, customerId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFields.PurchaseDate, customerId, settingNames, settings);
        }

        private static void CreateMissingOperatingSystemFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(OperatingSystemFields.OS, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.Version, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.ServicePack, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.RegistrationCode, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.ProductKey, customerId, settingNames, settings);
        }

        private static void CreateMissingProcessorFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(ProcessorFields.ProccesorName, customerId, settingNames, settings);
        }

        private static void CreateMissingMemoryFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(MemoryFields.RAM, customerId, settingNames, settings);
        }

        private static void CreateMissingCommunicationFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(CommunicationFields.NetworkAdapter, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.IPAddress, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.MacAddress, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.RAS, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.NovellClient, customerId, settingNames, settings);
        }

        private static void CreateMissingGraphicsFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(GraphicsFields.VideoCard, customerId, settingNames, settings);
        }

        private static void CreateMissingSoundFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(SoundFields.SoundCard, customerId, settingNames, settings);
        }

        private static void CreateMissingContractFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(ContractFields.ContractStatusName, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.ContractNumber, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.ContractStartDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.ContractEndDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.WarrantyEndDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.PurchasePrice, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.AccountingDimension1, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.AccountingDimension2, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.AccountingDimension3, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.AccountingDimension4, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.AccountingDimension5, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContractFields.Document, customerId, settingNames, settings);
        }

        private static void CreateMissingOtherFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(OtherFields.Info, customerId, settingNames, settings);
        }

        private static void CreateMissingContactInformationFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(ContactInformationFields.UserId, customerId, settingNames, settings);
        }

        private static void CreateMissingOrganizationFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(OrganizationFields.Region, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrganizationFields.Department, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrganizationFields.Domain, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrganizationFields.Unit, customerId, settingNames, settings);
        }

        private static void CreateMissingPlaceFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(PlaceFields.Room, customerId, settingNames, settings);
            CreateSettingIfNeeded(PlaceFields.Address, customerId, settingNames, settings);
            CreateSettingIfNeeded(PlaceFields.PostalCode, customerId, settingNames, settings);
            CreateSettingIfNeeded(PlaceFields.PostalAddress, customerId, settingNames, settings);
            CreateSettingIfNeeded(PlaceFields.Location, customerId, settingNames, settings);
            CreateSettingIfNeeded(PlaceFields.Location2, customerId, settingNames, settings);
        }

        private static void CreateMissingContactFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(ContactFields.Name, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContactFields.Phone, customerId, settingNames, settings);
            CreateSettingIfNeeded(ContactFields.Email, customerId, settingNames, settings);
        }

        private static void CreateMissingStateFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(StateFields.State, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(StateFields.Stolen, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(StateFields.Replaced, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(StateFields.SendBack, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(StateFields.ScrapDate, customerId, settingNames, settings, false);
        }

        private static void CreateMissingDateFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings)
        {
            CreateSettingIfNeeded(DateFields.CreatedDate, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(DateFields.ChangedDate, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(DateFields.SynchronizeDate, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(DateFields.ScanDate, customerId, settingNames, settings, false);
            CreateSettingIfNeeded(DateFields.PathDirectory, customerId, settingNames, settings, false);
        }

        private static void CreateSettingIfNeeded(
            string checkSettingName,
            int customerId,
            List<string> settingNames,
            DbSet<ComputerFieldSettings> settings, 
            bool isCopy = true)
        {
            var settingExists = settingNames.Any(s => s.ToLower() == checkSettingName.ToLower());
            if (settingExists)
            {
                return;
            }

            var setting = CreateDefaultSetting(checkSettingName, customerId, isCopy);
            settings.Add(setting);
        }

        #endregion
    }
}