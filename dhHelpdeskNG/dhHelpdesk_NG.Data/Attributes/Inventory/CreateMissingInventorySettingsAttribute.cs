namespace DH.Helpdesk.Dal.Attributes.Inventory
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DH.Helpdesk.Dal.Enums.Inventory.Inventory;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Inventory;

    using PostSharp.Aspects;

    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CreateMissingInventorySettingsAttribute : OnMethodBoundaryAspect
    {
        #region Fields

        private readonly string inventoryTypeIdName;

        #endregion

        #region Constructors and Destructors

        public CreateMissingInventorySettingsAttribute(string inventoryTypeId)
        {
            this.inventoryTypeIdName = inventoryTypeId;
        }

        #endregion

        #region Public Methods and Operators

        public override void OnEntry(MethodExecutionArgs args)
        {
            var methodParameters = args.Method.GetParameters();
            var inventoryTypeIdParameter = methodParameters.Single(p => p.Name == this.inventoryTypeIdName);
            var inventoryTypeIdParameterIndex = inventoryTypeIdParameter.Position;
            var inventoryTypeId = (int)args.Arguments[inventoryTypeIdParameterIndex];

            var dbcontext = new DatabaseFactory().Get();
            var settings = dbcontext.InventoryTypeProperties;
            var settingNames = settings.Where(s => s.InventoryType_Id == inventoryTypeId).Select(s => s.PropertyValue).ToList();

            CreateMissingWorkstationFieldsSettings(inventoryTypeId, settingNames, settings);

            dbcontext.Commit();
            base.OnEntry(args);
        }

        #endregion

        #region Methods

        private static InventoryTypeProperty CreateDefaultSetting(
            string fieldName,
            int propertyType,
            int inventoryTypeId)
        {
            return new InventoryTypeProperty
                       {
                           PropertyValue = fieldName,
                           CreatedDate = DateTime.Now,
                           ChangedDate = DateTime.Now, // todo
                           InventoryType_Id = inventoryTypeId,
                           Show = 0,
                           ShowInList = 0,
                           PropertySize = 50,
                           PropertyPos = 0,
                           InventoryTypeGroup_Id = null,
                           PropertyType = propertyType,
                       };
        }

        private static void CreateMissingWorkstationFieldsSettings(
            int inventoryTypeId,
            List<string> settingNames,
            DbSet<InventoryTypeProperty> settings)
        {
            CreateSettingIfNeeded(InventoryFieldNames.Department, InventoryFields.Department, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.Name, InventoryFields.Name, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.Model, InventoryFields.Model, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.Manufacturer, InventoryFields.Manufacturer, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.SerialNumber, InventoryFields.SerialNumber, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.TheftMark, InventoryFields.TheftMark, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.BarCode, InventoryFields.BarCode, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.PurchaseDate, InventoryFields.PurchaseDate, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.Place, InventoryFields.Place, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.Workstation, InventoryFields.Workstation, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.Info, InventoryFields.Info, inventoryTypeId, settingNames, settings);
            CreateSettingIfNeeded(InventoryFieldNames.Type, InventoryFields.Type, inventoryTypeId, settingNames, settings);
        }

        private static void CreateSettingIfNeeded(
            string checkSettingName,
            int propertyType,
            int inventoryTypeId,
            List<string> settingNames,
            DbSet<InventoryTypeProperty> settings)
        {
            var settingExists = settingNames.Any(s => s.ToLower() == checkSettingName.ToLower());
            if (settingExists)
            {
                return;
            }

            var setting = CreateDefaultSetting(checkSettingName, propertyType, inventoryTypeId);
            settings.Add(setting);
        }

        #endregion
    }
}