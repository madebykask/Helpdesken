namespace DH.Helpdesk.Dal.Attributes.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DH.Helpdesk.Dal.Enums.Inventory.Server;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Servers;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingServerSettingsAttribute : OnMethodBoundaryAspect
    {
        #region Fields

        private readonly string customerIdParameterName;

        #endregion

        #region Constructors and Destructors

        public CreateMissingServerSettingsAttribute(string customerIdParameter)
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
            var settings = dbcontext.ServerFieldSettings;
            var settingNames = settings.Where(s => s.Customer_Id == customerId).Select(s => s.ServerField).ToList();

            CreateMissingGeneralFieldsSettings(customerId, settingNames, settings);
            CreateMissingChassisFieldsSettings(customerId, settingNames, settings);
            CreateMissingInventoringFieldsSettings(customerId, settingNames, settings);
            CreateMissingOperatingSystemFieldsSettings(customerId, settingNames, settings);
            CreateMissingProcessorFieldsSettings(customerId, settingNames, settings);
            CreateMissingMemoryFieldsSettings(customerId, settingNames, settings);
            CreateMissingCommunicationFieldsSettings(customerId, settingNames, settings);
            CreateMissingStorageFieldsSettings(customerId, settingNames, settings);
            CreateMissingOtherFieldsSettings(customerId, settingNames, settings);
            CreateMissingPlaceFieldsSettings(customerId, settingNames, settings);
            CreateMissingStateFieldsSettings(customerId, settingNames, settings);
            CreateMissingDocumentFieldsSettings(customerId, settingNames, settings);

            dbcontext.Commit();
            base.OnEntry(args);
        }

        #endregion

        #region Methods

        private static ServerFieldSettings CreateDefaultSetting(string fieldName, int customerId)
        {
            return new ServerFieldSettings
                   {
                       ServerField = fieldName,
                       CreatedDate = DateTime.Now,
                       ChangedDate = DateTime.Now, // todo
                       Customer_Id = customerId,
                       Label = fieldName,
                       Label_ENG = fieldName,
                       Required = 0,
                       Show = 0,
                       ShowInList = 0,
                       FieldHelp = string.Empty,
                   };
        }

        private static void CreateMissingGeneralFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(GeneralFields.Name, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralFields.Manufacturer, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralFields.Description, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralFields.Model, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralFields.SerialNumber, customerId, settingNames, settings);
        }

        private static void CreateMissingChassisFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(ChassisFields.Chassis, customerId, settingNames, settings);
        }

        private static void CreateMissingInventoringFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(InventoryFields.BarCode, customerId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFields.PurchaseDate, customerId, settingNames, settings);
        }

        private static void CreateMissingOperatingSystemFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(OperatingSystemFields.OperatingSystem, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.Version, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.ServicePack, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.RegistrationCode, customerId, settingNames, settings);
            CreateSettingIfNeeded(OperatingSystemFields.ProductKey, customerId, settingNames, settings);
        }

        private static void CreateMissingProcessorFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(ProcessorFields.ProccesorName, customerId, settingNames, settings);
        }

        private static void CreateMissingMemoryFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(MemoryFields.RAM, customerId, settingNames, settings);
        }

        private static void CreateMissingStorageFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(StorageFields.Capasity, customerId, settingNames, settings);
        }

        private static void CreateMissingCommunicationFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(CommunicationFields.NetworkAdapter, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.IPAddress, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.MacAddress, customerId, settingNames, settings);
        }

        private static void CreateMissingOtherFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(OtherFields.Info, customerId, settingNames, settings);
            CreateSettingIfNeeded(OtherFields.Other, customerId, settingNames, settings);
            CreateSettingIfNeeded(OtherFields.URL, customerId, settingNames, settings);
            CreateSettingIfNeeded(OtherFields.URL2, customerId, settingNames, settings);
            CreateSettingIfNeeded(OtherFields.Owner, customerId, settingNames, settings);
        }

        private static void CreateMissingPlaceFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(PlaceFields.Room, customerId, settingNames, settings);
            CreateSettingIfNeeded(PlaceFields.Location, customerId, settingNames, settings);
        }

        private static void CreateMissingStateFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(StateFields.CreatedDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(StateFields.ChangedDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(StateFields.SyncChangeDate, customerId, settingNames, settings);
        }

        private static void CreateMissingDocumentFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            CreateSettingIfNeeded(DocumentFields.Document, customerId, settingNames, settings);
        }

        private static void CreateSettingIfNeeded(
            string checkSettingName,
            int customerId,
            List<string> settingNames,
            DbSet<ServerFieldSettings> settings)
        {
            var settingExists = settingNames.Any(s => s.ToLower() == checkSettingName.ToLower());
            if (settingExists)
            {
                return;
            }

            var setting = CreateDefaultSetting(checkSettingName, customerId);
            settings.Add(setting);
        }

        #endregion
    }
}