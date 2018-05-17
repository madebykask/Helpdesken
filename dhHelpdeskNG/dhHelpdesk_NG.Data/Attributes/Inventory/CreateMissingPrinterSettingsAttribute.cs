namespace DH.Helpdesk.Dal.Attributes.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DH.Helpdesk.Dal.Enums.Inventory.Printer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Printers;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingPrinterSettingsAttribute : OnMethodBoundaryAspect
    {
        #region Fields

        private readonly string customerIdParameterName;

        #endregion

        #region Constructors and Destructors

        public CreateMissingPrinterSettingsAttribute(string customerIdParameter)
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
            var settings = dbcontext.PrinterFieldSettings;
            var settingNames = settings.Where(s => s.Customer_Id == customerId).Select(s => s.PrinterField).ToList();

            CreateMissingGeneralFieldsSettings(customerId, settingNames, settings);
            CreateMissingInventoringFieldsSettings(customerId, settingNames, settings);
            CreateMissingOrganizationFieldsSettings(customerId, settingNames, settings);
            CreateMissingCommunicationFieldsSettings(customerId, settingNames, settings);
            CreateMissingOtherFieldsSettings(customerId, settingNames, settings);
            CreateMissingPlaceFieldsSettings(customerId, settingNames, settings);
            CreateMissingStateFieldsSettings(customerId, settingNames, settings);

            dbcontext.Commit();
            base.OnEntry(args);
        }

        #endregion

        #region Methods

        private static PrinterFieldSettings CreateDefaultSetting(string fieldName, int customerId)
        {
            return new PrinterFieldSettings
                   {
                       PrinterField = fieldName,
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
            DbSet<PrinterFieldSettings> settings)
        {
            CreateSettingIfNeeded(GeneralFields.Name, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralFields.Manufacturer, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralFields.Model, customerId, settingNames, settings);
            CreateSettingIfNeeded(GeneralFields.SerialNumber, customerId, settingNames, settings);
        }

        private static void CreateMissingInventoringFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<PrinterFieldSettings> settings)
        {
            CreateSettingIfNeeded(InventoryFields.BarCode, customerId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFields.PurchaseDate, customerId, settingNames, settings);
        }

        private static void CreateMissingCommunicationFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<PrinterFieldSettings> settings)
        {
            CreateSettingIfNeeded(CommunicationFields.NetworkAdapter, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.IPAddress, customerId, settingNames, settings);
            CreateSettingIfNeeded(CommunicationFields.MacAddress, customerId, settingNames, settings);
        }

        private static void CreateMissingOtherFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<PrinterFieldSettings> settings)
        {
            CreateSettingIfNeeded(OtherFields.NumberOfTrays, customerId, settingNames, settings);
            CreateSettingIfNeeded(OtherFields.Driver, customerId, settingNames, settings);
            CreateSettingIfNeeded(OtherFields.Info, customerId, settingNames, settings);
            CreateSettingIfNeeded(OtherFields.URL, customerId, settingNames, settings);
        }

        private static void CreateMissingOrganizationFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<PrinterFieldSettings> settings)
        {
            CreateSettingIfNeeded(OrganizationFields.Department, customerId, settingNames, settings);
            CreateSettingIfNeeded(OrganizationFields.Unit, customerId, settingNames, settings);
        }

        private static void CreateMissingPlaceFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<PrinterFieldSettings> settings)
        {
            CreateSettingIfNeeded(PlaceFields.Room, customerId, settingNames, settings);
            CreateSettingIfNeeded(PlaceFields.Location, customerId, settingNames, settings);
        }

        private static void CreateMissingStateFieldsSettings(
            int customerId,
            List<string> settingNames,
            DbSet<PrinterFieldSettings> settings)
        {
            CreateSettingIfNeeded(StateFields.CreatedDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(StateFields.ChangedDate, customerId, settingNames, settings);
            CreateSettingIfNeeded(StateFields.SyncDate, customerId, settingNames, settings);
        }

        private static void CreateSettingIfNeeded(
            string checkSettingName,
            int customerId,
            List<string> settingNames,
            DbSet<PrinterFieldSettings> settings)
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