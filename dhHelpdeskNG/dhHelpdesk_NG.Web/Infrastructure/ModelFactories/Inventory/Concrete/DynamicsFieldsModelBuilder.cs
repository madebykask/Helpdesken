namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.InventorySettings;
    using DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory;

    public class DynamicsFieldsModelBuilder : IDynamicsFieldsModelBuilder
    {
        public List<DynamicFieldModel> BuildViewModel(
            List<InventoryValue> dynamicData,
            List<InventoryDynamicFieldSettingForModelEdit> settings,
            int inventoryId)
        {
            var data = new List<DynamicFieldModel>();

            foreach (var setting in settings)
            {
                var dynamicField =
                    dynamicData.SingleOrDefault(
                        x => x.InventoryTypePropertyId == setting.Id && x.InventoryId == inventoryId);

                var dynamicFieldModel = new DynamicFieldModel(
                    setting.Id,
                    setting.InventoryTypeGroupId ?? 0,
                    setting.Caption,
                    setting.Position,
                    setting.PropertySize,
                    setting.FieldType,
                    ConvertValue(setting.FieldType, dynamicField != null ? dynamicField.Value : null));

                data.Add(dynamicFieldModel);
            }

            return data;
        }

        public List<DynamicFieldModel> BuildViewModel(List<InventoryDynamicFieldSettingForModelEdit> settings)
        {
            var data = new List<DynamicFieldModel>();

            foreach (var setting in settings)
            {
                var dynamicFieldModel = new DynamicFieldModel(
                    setting.Id,
                    setting.InventoryTypeGroupId ?? 0,
                    setting.Caption,
                    setting.Position,
                    setting.PropertySize,
                    setting.FieldType,
                    null);

                data.Add(dynamicFieldModel);
            }

            return data;
        }

        private static object ConvertValue(FieldTypes fieldType, string value)
        {
            switch (fieldType)
            {
                case FieldTypes.Bool:
                    return !string.IsNullOrWhiteSpace(value) && value == DynamicFieldModel.True;
                case FieldTypes.Date:
                    return !string.IsNullOrWhiteSpace(value) ? (DateTime?)DateTime.Parse(value) : null;
                case FieldTypes.Text:
                    return value;
                case FieldTypes.Numeric:
                    return !string.IsNullOrWhiteSpace(value) ? (int?)int.Parse(value) : 12;
                default:
                    return value;
            }
        }
    }
}